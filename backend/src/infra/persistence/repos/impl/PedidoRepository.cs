using backend.domain;
using backend.infra.repos.contracts;
using Microsoft.EntityFrameworkCore;

namespace backend.infra.repos.impl
{
    public sealed class PedidoRepository : IPedidoRepository
    {
        private readonly PedidoContext _context;
        public PedidoRepository(PedidoContext context)
        {
            _context = context;
        }
        public async Task<bool> AtualizarStatusPedidoAsync(Pedido pedido)
        {
            foreach (var historico in pedido.HistoricoStatus)
            {
                var entry = _context.Entry(historico);
                // Pelo fato do objeto de dominio históricoStatus gerar um novo Id, o EF não consegue
                // identificar que o objeto é novo, então precisamos setar o estado dele como Added
                if (entry.State == EntityState.Detached)
                {
                    entry.State = EntityState.Added;
                }
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Guid> CriarPedidoAsync(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return pedido.Id;
        }

        public async Task<IList<Pedido>> ObterPedidosPorCursorAsync(DateTime cursor,Guid? ultimoId, Guid? usuarioId, int limite = 10)
        {
            var query = _context.Pedidos
                .Include(p => p.Itens).ThenInclude(i => i.Produto)
                .Where(p => p.DataCriacao < cursor || (p.DataCriacao == cursor && ultimoId.HasValue && p.Id.CompareTo(ultimoId.Value) < 0))
                .OrderByDescending(p => p.DataCriacao)
                .ThenByDescending(p => p.Id)
                .Take(limite);

            if (usuarioId.HasValue)
                query = query.Where(p => p.IdCliente == usuarioId.Value);


            return await query.ToListAsync();
        }
        public async Task<Pedido?> ObterPedidoComItensEProdutosPorIdAsync(Guid idPedido)
        {
            var query = _context.Pedidos
                .Include(p => p.Itens).ThenInclude(i => i.Produto)
                .Where(p => p.Id == idPedido);

            return await query.FirstOrDefaultAsync();
        }
    }
}