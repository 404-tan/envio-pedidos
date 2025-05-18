using backend.domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
public class PedidoContext : IdentityDbContext<Usuario, Papel, Guid>
{
    public PedidoContext(DbContextOptions<PedidoContext> options) : base(options) { }

    
}