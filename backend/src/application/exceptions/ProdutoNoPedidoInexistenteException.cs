using System;

namespace backend.application.exceptions
{
    public class ProdutoNoPedidoInexistenteException : Exception
    {
        public ProdutoNoPedidoInexistenteException(string id)
            : base($"O produto com o id {id} não existe no sistema.") { }
    }
}