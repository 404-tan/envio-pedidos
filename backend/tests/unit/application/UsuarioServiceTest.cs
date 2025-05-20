using Moq;
using backend.application.DTOs.requests;
using backend.application.exceptions;
using backend.application.services.impl;
using backend.domain;
using backend.infra.security.contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.tests.unit.application;

public class UsuarioServiceTests
{
    private readonly Mock<UserManager<Usuario>> _userManagerMock;
    private readonly Mock<RoleManager<Papel>> _roleManagerMock;
    private readonly Mock<IUsuarioAutenticado> _usuarioAutenticadoMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly UsuarioService _service;

    public UsuarioServiceTests()
    {
        _userManagerMock = new Mock<UserManager<Usuario>>(
            Mock.Of<IUserStore<Usuario>>(), null, null, null, null, null, null, null, null);

        _roleManagerMock = new Mock<RoleManager<Papel>>(
            Mock.Of<IRoleStore<Papel>>(), null, null, null, null);

        _usuarioAutenticadoMock = new Mock<IUsuarioAutenticado>();
        _configMock = new Mock<IConfiguration>();

        _configMock.Setup(c => c["JwtSettings:SecretKey"]).Returns("chavesecretaultra12345678901234567890");
        _configMock.Setup(c => c["JwtSettings:Issuer"]).Returns("test-issuer");
        _configMock.Setup(c => c["JwtSettings:Audience"]).Returns("test-audience");
        _configMock.Setup(c => c["JwtSettings:ExpiresInMinutes"]).Returns("30");

        _service = new UsuarioService(
            _userManagerMock.Object,
            _usuarioAutenticadoMock.Object,
            _roleManagerMock.Object,
            _configMock.Object
        );
    }

    [Fact]
    public async Task LogarUsuarioAsync_DeveRetornarUsuarioResponse_SeCredenciaisForemValidas()
    {
        var usuario = Usuario.Criar("Usuário Teste", "teste@email.com");
        _userManagerMock.Setup(u => u.FindByEmailAsync(usuario.Email)).ReturnsAsync(usuario);
        _userManagerMock.Setup(u => u.CheckPasswordAsync(usuario, "senha123")).ReturnsAsync(true);
        _userManagerMock.Setup(u => u.GetRolesAsync(usuario)).ReturnsAsync(new List<string> { "Cliente" });

        var request = new LoginRequest(usuario.Email, "senha123");

        var result = await _service.LogarUsuarioAsync(request);

        result.Should().NotBeNull();
        result.Email.Should().Be(usuario.Email);
        result.NomeCompleto.Should().Be(usuario.NomeCompleto);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task LogarUsuarioAsync_DeveLancarExcecao_SeEmailNaoExistir()
    {
        _userManagerMock.Setup(u => u.FindByEmailAsync("inexistente@email.com")).ReturnsAsync((Usuario)null);

        var request = new LoginRequest("inexistente@email.com", "123");

        Func<Task> act = async () => await _service.LogarUsuarioAsync(request);

        await act.Should().ThrowAsync<UsuarioOuSenhaInvalidosException>();
    }

    [Fact]
    public async Task LogarUsuarioAsync_DeveLancarExcecao_SeSenhaForInvalida()
    {
        var usuario = Usuario.Criar("Teste", "email@email.com");
        _userManagerMock.Setup(u => u.FindByEmailAsync(usuario.Email)).ReturnsAsync(usuario);
        _userManagerMock.Setup(u => u.CheckPasswordAsync(usuario, "123")).ReturnsAsync(false);

        var request = new LoginRequest(usuario.Email, "123");

        Func<Task> act = async () => await _service.LogarUsuarioAsync(request);

        await act.Should().ThrowAsync<UsuarioOuSenhaInvalidosException>();
    }

    [Fact]
    public async Task RegistrarUsuarioAsync_DeveLancarExcecao_SeUsuarioJaExiste()
    {
        var usuarioExistente = Usuario.Criar("Existente", "existente@email.com");
        _userManagerMock.Setup(u => u.FindByEmailAsync(usuarioExistente.Email)).ReturnsAsync(usuarioExistente);

        var request = new RegistroRequest(usuarioExistente.NomeCompleto, usuarioExistente.Email, "senha123");

        Func<Task> act = async () => await _service.RegistrarUsuarioAsync(request);

        await act.Should().ThrowAsync<UsuarioJaExisteException>();
    }

    [Fact]
    public async Task RegistrarUsuarioAsync_DeveLancarExcecao_SeCriacaoFalhar()
    {
        _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((Usuario)null);
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<Usuario>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Senha fraca" }));

        var request = new RegistroRequest("Novo Usuário", "novo@email.com", "123");

        Func<Task> act = async () => await _service.RegistrarUsuarioAsync(request);

        await act.Should().ThrowAsync<FalhaAoCriarUsuarioException>()
            .WithMessage("*Senha fraca*");
    }

    [Fact]
    public async Task RegistrarUsuarioAsync_DeveRegistrarComSucesso()
    {
        _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((Usuario)null);
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<Usuario>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _roleManagerMock.Setup(r => r.RoleExistsAsync("Cliente")).ReturnsAsync(true);
        _userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<Usuario>(), "Cliente")).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<Usuario>())).ReturnsAsync(new List<string> { "Cliente" });

        var request = new RegistroRequest("Novo Usuário", "novo@email.com", "senha123");

        var result = await _service.RegistrarUsuarioAsync(request);

        result.Should().NotBeNull();
        result.Email.Should().Be(request.Email);
        result.NomeCompleto.Should().Be(request.NomeCompleto);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void ObterIdUsuarioAutenticado_DeveRetornarId()
    {
        var userId = Guid.NewGuid();
        _usuarioAutenticadoMock.Setup(x => x.ObterId()).Returns(userId);

        var result = _service.ObterIdUsuarioAutenticado();

        result.Should().Be(userId);
    }

    [Fact]
    public void ObterIdUsuarioAutenticado_DeveLancarExcecao_SeNaoAutenticado()
    {
        _usuarioAutenticadoMock.Setup(x => x.ObterId()).Returns(Guid.Empty);

        Action act = () => _service.ObterIdUsuarioAutenticado();

        act.Should().Throw<UsuarioNaoAutenticadoException>();
    }

    [Fact]
    public async Task ObterNomeCompletoUsuarioPorIdAsync_DeveRetornarNome()
    {
        var usuario = Usuario.Criar("Nome Completo", "teste@email.com");
        _userManagerMock.Setup(x => x.FindByIdAsync(usuario.Id.ToString())).ReturnsAsync(usuario);

        var result = await _service.ObterNomeCompletoUsuarioPorIdAsync(usuario.Id);

        result.Should().Be("Nome Completo");
    }

    [Fact]
    public async Task ObterNomeCompletoUsuarioPorIdAsync_DeveLancarExcecao_SeUsuarioNaoExiste()
    {
        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((Usuario)null);

        Func<Task> act = async () => await _service.ObterNomeCompletoUsuarioPorIdAsync(Guid.NewGuid());

        await act.Should().ThrowAsync<UsuarioNaoExisteException>();
    }
}