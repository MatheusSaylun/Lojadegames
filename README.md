# 🎮 Loja de Games

Sistema de gerenciamento de vendas para uma loja de games, desenvolvido em **C# com .NET** e banco de dados **SQLite**.  
Organizado em camadas com **Models**, **Repositories** e **Services**, seguindo boas práticas de arquitetura.

---

## 📦 Funcionalidades

- ✅ Cadastro de usuários (com validação de CPF, e-mail e telefone)
- ✅ Login seguro com senha criptografada (BCrypt)
- ✅ Cadastro de clientes
- ✅ Cadastro e busca de produtos por código de barras
- ✅ Registro de vendas com:
  - Código único
  - Itens vendidos
  - Formas de pagamento
  - Controle de status (Pagamento/Venda)
- ✅ Validação de supervisor para ações críticas

---

## 🗂 Estrutura do Projeto

Lojadegames/
├── Models/ # Entidades: Usuario, Cliente, Produto, Venda...
├── Repositories/ # Acesso ao banco de dados SQLite
├── Services/ # Lógica de negócios (ex: VendaService)
├── Program.cs # Ponto de entrada da aplicação
├── usuarios.db # Banco de dados SQLite
└── README.md # Este arquivo

---

## 🧪 Tecnologias Utilizadas

- 💻 **C#**
- 🧱 **.NET**
- 🐿 **SQLite**
- 🔐 **BCrypt.Net** (para senhas)
- 📁 Organização por camadas (MVC simples)

---
🛠 Como executar

Requisitos:
.NET SDK instalado no seu computador

1. Clone o repositório ou baixe o projeto:

git clone https://github.com/seu-usuario/lojadegames.git
cd lojadegames

  Ou baixe o .zip pelo botão verde "Code > Download ZIP" e extraia no seu computador.

2. Abra a pasta no terminal e execute o projeto com:

  dotnet run

3. Pronto! O sistema será iniciado no terminal.

  O banco de dados (usuarios.db) será criado automaticamente na primeira execução.

