using System;

namespace backend.application.exceptions
{
    public class FalhaAoCriarUsuarioException : Exception
    {
        public FalhaAoCriarUsuarioException(string message)
            : base(message) { }
    }
}