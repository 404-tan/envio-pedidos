using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace backend.domain
{
    public sealed class Usuario(string nomeCompleto) : IdentityUser<Guid>
    {
        public string NomeCompleto { get; private set; } = nomeCompleto;
        public ICollection<Pedido> Pedidos { get; private set; } = [];

        public static Usuario Create(string nomeCompleto)
        {
            if (string.IsNullOrWhiteSpace(nomeCompleto))
                throw new ArgumentException("O nome completo não pode ser vazio.");

            return new Usuario(nomeCompleto);
        }
    }
}