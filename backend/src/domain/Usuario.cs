using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace backend.domain
{
    public sealed class Usuario : IdentityUser<Guid>
    {
        public string NomeCompleto { get; private set; }
        public ICollection<Pedido> PedidosFeitos { get; private set; } = [];
        public ICollection<Pedido> PedidosProcessados { get; private set; } = [];
        private Usuario() { }

        private Usuario(string nomeCompleto, string email, string userName)
        {
            NomeCompleto = nomeCompleto;
            Email = email;
            UserName = userName;
        }

        public static Usuario Criar(string nomeCompleto,string email)
        {
            if (string.IsNullOrWhiteSpace(nomeCompleto))
                throw new ArgumentException("O nome completo não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("O email não pode ser vazio.");
            return new Usuario(
                nomeCompleto,
                email,
                email 
            );
        }
    }
}