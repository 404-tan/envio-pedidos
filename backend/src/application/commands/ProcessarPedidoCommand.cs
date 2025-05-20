namespace backend.application.commands;

public record ProcessarPedidoCommand(
    Guid IdPedido,
    Guid IdAdministrador,
    string NomeAdministrador
);