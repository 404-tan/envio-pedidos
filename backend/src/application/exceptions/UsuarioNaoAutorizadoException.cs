using System;

namespace backend.application.exceptions
{
    public class UsuarioNaoAutorizadoException : Exception
    {
        public UsuarioNaoAutorizadoException()
            : base("Usuário não autorizado a processar pedidos.") { }
    }
}