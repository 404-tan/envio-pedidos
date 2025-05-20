using System;

namespace backend.application.exceptions
{
    public class UsuarioOuSenhaInvalidosException : Exception
    {
        public UsuarioOuSenhaInvalidosException()
            : base("Usuário ou senha inválidos.") { }
    }
}