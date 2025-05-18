using System.Security.Claims;
using backend.infra.security.contracts;
namespace backend.infra.security.impl;
public class UsuarioAutenticado : IUsuarioAutenticado
{
    private readonly IHttpContextAccessor _accessor;

    public UsuarioAutenticado(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public Guid ObterId()
    {
        var id = _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(id ?? throw new UnauthorizedAccessException());
    }
}