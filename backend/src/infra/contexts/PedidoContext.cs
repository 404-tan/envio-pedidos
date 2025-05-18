using backend.domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
public class PedidoContext : IdentityDbContext<Usuario, Papel, Guid>
{
    public PedidoContext(DbContextOptions<PedidoContext> options) : base(options) { }

    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<HistoricoStatusPedido> HistoricoStatusPedidos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Pedido>()
            .HasMany(p => p.HistoricoStatus)
            .WithOne()
            .HasForeignKey(h => h.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}