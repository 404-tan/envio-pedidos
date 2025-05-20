#  Envio de Pedidos

Aplicação full-stack simples para cadastro, listagem e processamento de pedidos com Angular + ASP.NET Core + RabbitMQ + SQL Server.

##  Rodando o projeto

Tudo que você precisa é de **Docker** e **Docker Compose**.

```bash
docker-compose up --build
```

Isso vai subir os seguintes serviços:

- `api` – backend ASP.NET rodando em `http://localhost:5000`
-  `sqlserver` – banco de dados SQL Server
-  `rabbitmq` – fila para processamento assíncrono
-  `frontend` – app Angular servida via NGINX em `http://localhost:4200`

---

##  Acesso Rápido

###  Login de Administrador:

- **Email:** `admin@admin.com`  
- **Senha:** `Admin123!`

---

##  Rotas úteis

-  **Login:** [http://localhost:4200/login](http://localhost:4200/login)
-  **Registro:** [http://localhost:4200/registrar](http://localhost:4200/registrar)

---

## Estrutura do projeto

```
envio-pedidos/
├── backend/           # API ASP.NET Core com MassTransit e RabbitMQ
├── frontend/          # Angular 19
├── docker-compose.yml
└── README.md
```

---

##  Tecnologias usadas

- ASP.NET CORE 9
- Angular 19
- SQL Server
- RabbitMQ + MassTransit
- Docker & Docker Compose
- NGINX

---

##  Funcionalidades

-  Cadastro de pedidos com múltiplos itens
-  Processamento assíncrono via fila
-  Filtro por status (Criado, Processado)
-  Autenticação com JWT
-  Acesso restrito a administradores para processar pedidos
