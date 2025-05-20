using System;

namespace backend.application.exceptions
{
    public class FalhaAoCriarPedidoException : Exception
    {
        public FalhaAoCriarPedidoException(string message)
            : base(message) { }
    }
}