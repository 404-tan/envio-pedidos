using System;

namespace backend.application.exceptions
{
    public class PedidoNaoEncontradoException : Exception
    {
        public PedidoNaoEncontradoException()
            : base("Pedido não encontrado.") { }
    }
}