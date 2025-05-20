using System;

namespace backend.application.exceptions
{
    public class FalhaAtualizarPedidoException : Exception
    {
        public FalhaAtualizarPedidoException()
            : base("Erro ao atualizar o pedido.") { }
    }
}