using System;

namespace backend.application.exceptions
{
    public class UsuarioNaoExisteException : Exception
    {
        public UsuarioNaoExisteException(string id)
            : base($"O usuário com o id {id} não existe.") { }
    }
}