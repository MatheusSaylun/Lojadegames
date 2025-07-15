// Estrutura do projeto organizada em camadas:
// - Program.cs (apresentação)
// - Services/UsuarioService.cs (regras de negócio)
// - Repositories/UsuarioRepository.cs (acesso ao banco)
// - Models/Usuario.cs (dados)

// ===================== Program.cs =====================

using Lojadegames.Models;
using Lojadegames.Repositories;
using Lojadegames.Services;
using Microsoft.Data.Sqlite;
using static Lojadegames.Models.Usuario;
using static Lojadegames.Repositories.UsuarioRepository;

string? senha, confirmacao;

while (true)
{
    Console.WriteLine("---MENU---");
    Console.WriteLine("[1] Fazer o Login");
    Console.WriteLine("[2] Cadastro de usuario");
    Console.WriteLine("[0] Sair");

    if (!int.TryParse(Console.ReadLine(), out int escolhamenu))
    {
        Console.WriteLine("Entrada invalida. Digite 1, 2 ou 0");
        continue;
    }
    if (escolhamenu == 0) break;

    switch (escolhamenu)
    {
        case 1:
            Console.WriteLine("Email: ");
            string email = Console.ReadLine();
            Console.WriteLine("Senha: ");
            string senhaLogin = Console.ReadLine();
            var usuario = UsuarioService.FazerLogin(email, senhaLogin);
            if (usuario != null)
            {
                Console.WriteLine($"Bem Vindo {usuario.Nome}!");
                MenuInterno(usuario);
            }
            else
                Console.WriteLine("Email ou senha incorretos.");
            break;

        case 2:
            UsuarioService.CadastrarUsuario();
            break;

        default:
            Console.WriteLine("Opção inválida.");
            break;
    }
}
void MenuInterno(Usuario usuario)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine($"--- MENU PRINCIPAL --- | Usuário: {usuario.Nome} {(usuario.Ehsupervisor ? "(Supervisor)" : "")}");
        Console.WriteLine("[1] Cadastrar produto");
        Console.WriteLine("[2] Cadastrar cliente");
        Console.WriteLine("[3] Realizar Venda");
        Console.WriteLine("[0] Voltar");
        Console.Write("Escolha: ");

        if (!int.TryParse(Console.ReadLine(), out int opcao))
        {
            Console.WriteLine("Opção inválida. Pressione Enter...");
            Console.ReadLine();
            continue;
        }

        switch (opcao)
        {
            case 1:
                ProdutoService.CadastrarProduto();
                break;
            case 2:
                ClienteService.CadastrarCliente();
                break;
            case 3:
                VendaService.RealizarVenda(usuario);
                break;
            case 0:
                Console.WriteLine("Saindo...");
                return; // volta para o menu principal
            default:
                Console.WriteLine("Opção inválida. Pressione Enter...");
                Console.ReadLine();
                break;
        }
    }
}