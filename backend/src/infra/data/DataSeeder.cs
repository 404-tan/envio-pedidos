using backend.domain;
using backend.infra.repos.contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.infra.data;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<PedidoContext>();
        var roleManager = services.GetRequiredService<RoleManager<Papel>>();

        db.Database.Migrate();


        var roles = new[] { "Admin", "Cliente" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new Papel { Name = role });
            }
        }


        if (!await db.Produtos.AnyAsync())
        {
            var produtos = new[]
            {
                Produto.Criar("Pneu Michelin 205/55 R16", 529.90m),
                Produto.Criar("Jogo de Rodas Esportivas Aro 17", 1999.99m),
                Produto.Criar("Filtro de Óleo Fram PH5197", 29.90m),
                Produto.Criar("Óleo de Motor Mobil Super 5W30 1L", 42.50m),
                Produto.Criar("Bateria Moura 60Ah", 479.00m),
                Produto.Criar("Pastilha de Freio Dianteira Bosch", 145.00m),
                Produto.Criar("Kit Correia Dentada Gates", 298.90m),
                Produto.Criar("Lâmpada LED Farol H7 Philips", 89.90m),
                Produto.Criar("Sensor de Estacionamento Multilaser", 119.00m),
                Produto.Criar("Central Multimídia Pioneer 7\"", 1599.00m)
            };

            await db.Produtos.AddRangeAsync(produtos);
            await db.SaveChangesAsync();
        }

        var userManager = services.GetRequiredService<UserManager<Usuario>>();
        var adminEmail = "admin@admin.com";
        var adminSenha = "Admin123!";

        var usuarioExistente = await userManager.FindByEmailAsync(adminEmail);
        if (usuarioExistente == null)
        {
            var admin = Usuario.Criar("Administrador", adminEmail);
            var result = await userManager.CreateAsync(admin, adminSenha);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
