using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.application.DTOs.requests;
using backend.application.DTOs.responses;
using backend.application.services.contracts;
using backend.domain;
using backend.infra.security.contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace backend.application.services.impl
{
    public sealed class UsuarioService : IUsuarioService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<Papel> _roleManager;
        private readonly IUsuarioAutenticado _usuarioAutenticado;
        private readonly IConfiguration _config;
        public UsuarioService(UserManager<Usuario> userManager, IUsuarioAutenticado usuarioAutenticado, RoleManager<Papel> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = configuration;
            _usuarioAutenticado = usuarioAutenticado;
        }

        public async Task<bool> IsAdminAsync(Guid idUsuario)
        {
            var usuario = await _userManager.FindByIdAsync(idUsuario.ToString());
            if (usuario == null)
                return false;

            return await _userManager.IsInRoleAsync(usuario, "Admin");
        }

        public async Task<UsuarioResponse> LogarUsuarioAsync(LoginRequest request)
        {
            var usuario = await _userManager.FindByEmailAsync(request.Email);
            if (usuario == null)
                return null;

            var result = await _userManager.CheckPasswordAsync(usuario, request.SenhaDigitada);
            if (!result)
                return null;

            var token = await GerarJwtAsync(usuario);

            return new UsuarioResponse
            (
                usuario.Id,
                usuario.NomeCompleto,
                usuario.Email!,
                token
            );
        }

        public async Task<UsuarioResponse> RegistrarUsuarioAsync(RegistroRequest request)
        {
            var usuarioExistente = await _userManager.FindByEmailAsync(request.Email);
            if (usuarioExistente != null)
                return null;
            var usuario = Usuario.Criar(request.NomeCompleto, request.Email);
            var result = await _userManager.CreateAsync(usuario, request.SenhaDigitada);
            if (!result.Succeeded)
                return null;
            var papel = new Papel { Name = "Cliente" };
            if (!await _roleManager.RoleExistsAsync(papel.Name))
            {
                await _roleManager.CreateAsync(papel);
            }
            await _userManager.AddToRoleAsync(usuario, papel.Name);
            var token = await GerarJwtAsync(usuario);
            return new UsuarioResponse
            (
                usuario.Id,
                usuario.NomeCompleto,
                usuario.Email!,
                token
            );

        }
        public Guid ObterIdUsuarioAutenticado()
        {
            var idUsuario = _usuarioAutenticado.ObterId();
            if (idUsuario == Guid.Empty)
                throw new Exception("Usuário não autenticado");
            return idUsuario;
        }
        private async Task<string> GerarJwtAsync(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.UserName!),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(usuario);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["JwtSettings:ExpiresInMinutes"]!));

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> ObterNomeCompletoUsuarioPorIdAsync(Guid idUsuario)
        {
            var usuario = await _userManager.FindByIdAsync(idUsuario.ToString());
            if (usuario == null)
                return null;

            return usuario.NomeCompleto;
        }
    }

}