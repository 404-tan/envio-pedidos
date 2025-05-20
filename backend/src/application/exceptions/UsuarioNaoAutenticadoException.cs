using System;

namespace backend.application.exceptions
{
    public class UsuarioNaoAutenticadoException : Exception
    {
        public UsuarioNaoAutenticadoException()
            : base($"O usuário não está autenticado.") { }
    }
}