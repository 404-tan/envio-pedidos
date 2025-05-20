using backend.application.commands;
using backend.infra.repos.contracts;
using MassTransit;
namespace backend.infra.consumer;
public class ProcessarPedidoConsumer : IConsumer<ProcessarPedidoCommand>
{
    private readonly IPedidoRepository _pedidoRepository;

    public ProcessarPedidoConsumer(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task Consume(ConsumeContext<ProcessarPedidoCommand> context)
    {
        var message = context.Message;

        var pedido = await _pedidoRepository.ObterPedidoComItensEProdutosPorIdAsync(message.IdPedido);
        if (pedido == null)
            return; 

        pedido.Processar(message.IdAdministrador);

        var atualizado = await _pedidoRepository.AtualizarStatusPedidoAsync(pedido);
        if (!atualizado)
        {
            throw new Exception($"Falha ao processar o pedido {pedido.Id}.");
        }
    }
}