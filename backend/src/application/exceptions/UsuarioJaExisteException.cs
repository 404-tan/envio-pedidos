using System;

namespace backend.application.exceptions
{
    public class UsuarioJaExisteException : Exception
    {
        public UsuarioJaExisteException(string email)
            : base($"Já existe um usuário cadastrado com o e-mail '{email}'.") { }
    }
}