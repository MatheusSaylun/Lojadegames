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
                Console.WriteLine($"Bem-vindo {usuario.Nome}!");
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

// ===================== Models/Usuario.cs =====================

namespace Lojadegames.Models
{
    public class Usuario
    {
        public string? Nome { get; set; }
        public string? Senha { get; set; }
        public string? Email { get; set; }
        public string? Cpf { get; set; }
        public string? Telefone { get; set; }
        public bool Ehsupervisor { get; set; }

        public bool EmailValido() =>
            !string.IsNullOrEmpty(Email) &&
            System.Net.Mail.MailAddress.TryCreate(Email, out _);

        public bool CpfValido()
        {
            if (string.IsNullOrWhiteSpace(Cpf)) return false;
            string cpf = new string(Cpf.Where(char.IsDigit).ToArray());
            if (cpf.Length != 11 || cpf.Distinct().Count() == 1) return false;

            int Soma(int[] pesos) => cpf.Take(pesos.Length).Select((c, i) => (c - '0') * pesos[i]).Sum();
            int Resto(int soma) => soma % 11 < 2 ? 0 : 11 - (soma % 11);

            int[] peso1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] peso2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            return Resto(Soma(peso1)) == cpf[9] - '0' && Resto(Soma(peso2)) == cpf[10] - '0';
        }

        public bool TelefoneValido()
        {
            string tel = new string((Telefone ?? "").Where(char.IsDigit).ToArray());
            return tel.Length >= 10 && tel.Length <= 11;
        }
    }
}

// ===================== Repositories/UsuarioRepository.cs =====================

namespace Lojadegames.Repositories
{
    public static class UsuarioRepository
    {
        static string caminhoDb = "usuarios.db";

        public static void CriarTabelaSeNaoExistir()
        {
            using var conexao = new SqliteConnection($"Data Source={caminhoDb}");
            conexao.Open();
            var comando = conexao.CreateCommand();
            comando.CommandText = @"CREATE TABLE IF NOT EXISTS Usuarios (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nome TEXT, Senha TEXT, Email TEXT,
                Cpf TEXT, Telefone TEXT, Ehsupervisor INTEGER);
            ";
            comando.ExecuteNonQuery();
        }

        public static void Inserir(Usuario u)
        {
            using var conexao = new SqliteConnection($"Data Source={caminhoDb}");
            conexao.Open();
            var comando = conexao.CreateCommand();
            comando.CommandText = @"INSERT INTO Usuarios 
                (Nome, Senha, Email, Cpf, Telefone, Ehsupervisor) 
                VALUES ($nome, $senha, $email, $cpf, $telefone, $ehsupervisor);";

            comando.Parameters.AddWithValue("$nome", u.Nome);
            comando.Parameters.AddWithValue("$senha", BCrypt.Net.BCrypt.HashPassword(u.Senha));
            comando.Parameters.AddWithValue("$email", u.Email);
            comando.Parameters.AddWithValue("$cpf", u.Cpf);
            comando.Parameters.AddWithValue("$telefone", u.Telefone);
            comando.Parameters.AddWithValue("$ehsupervisor", u.Ehsupervisor ? 1 : 0);

            comando.ExecuteNonQuery();
        }

        public static Usuario? BuscarPorEmail(string email)
        {
            using var conexao = new SqliteConnection($"Data Source={caminhoDb}");
            conexao.Open();
            var comando = conexao.CreateCommand();
            comando.CommandText = @"SELECT Nome, Senha, Email, Cpf, Telefone, Ehsupervisor 
                                    FROM Usuarios WHERE Email = $email;";
            comando.Parameters.AddWithValue("$email", email);

            using var reader = comando.ExecuteReader();
            if (reader.Read())
            {
                return new Usuario
                {
                    Nome = reader.GetString(0),
                    Senha = reader.GetString(1),
                    Email = reader.GetString(2),
                    Cpf = reader.GetString(3),
                    Telefone = reader.GetString(4),
                    Ehsupervisor = reader.GetInt32(5) == 1
                };
            }
            return null;
        }
        public static class Utils
        {
            public static string LerSenhaOculta()
            {
                string senha = "";
                ConsoleKeyInfo tecla;

                do
                {
                    tecla = Console.ReadKey(true); // true: não exibe o caractere

                    if (tecla.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        break;
                    }
                    else if (tecla.Key == ConsoleKey.Backspace && senha.Length > 0)
                    {
                        senha = senha.Substring(0, senha.Length - 1);
                        Console.Write("\b \b");
                    }
                    else if (!char.IsControl(tecla.KeyChar))
                    {
                        senha += tecla.KeyChar;
                        Console.Write("*"); // mostra asterisco para cada caractere
                    }

                } while (true);

                return senha;
            }
        }

    }
}

// ===================== Services/UsuarioService.cs =====================

namespace Lojadegames.Services
{
    public static class UsuarioService
    {
        static UsuarioService()
        {
            UsuarioRepository.CriarTabelaSeNaoExistir();
        }

        public static void CadastrarUsuario()
        {
            Usuario novo = new Usuario();

            Console.WriteLine("Nome: "); novo.Nome = Console.ReadLine();

            string senha, confirmacao;
            do
            {
                Console.Write("Senha: ");
                senha = Utils.LerSenhaOculta();
                Console.Write("Confirme a senha: ");
                confirmacao = Utils.LerSenhaOculta();
                if (senha != confirmacao) Console.WriteLine("Senhas não coincidem!");
            } while (senha != confirmacao);
            novo.Senha = senha;

            do
            {
                Console.WriteLine("Email: "); novo.Email = Console.ReadLine();
                if (!novo.EmailValido()) Console.WriteLine("Email inválido!");
            } while (!novo.EmailValido());

            do
            {
                Console.WriteLine("CPF: "); novo.Cpf = Console.ReadLine();
                if (!novo.CpfValido()) Console.WriteLine("CPF inválido!");
            } while (!novo.CpfValido());

            do
            {
                Console.WriteLine("Telefone: "); novo.Telefone = Console.ReadLine();
                if (!novo.TelefoneValido()) Console.WriteLine("Telefone inválido!");
            } while (!novo.TelefoneValido());

            while (true)
            {
                Console.WriteLine("É supervisor? (true/false): ");
                if (bool.TryParse(Console.ReadLine(), out bool sup))
                {
                    novo.Ehsupervisor = sup;
                    break;
                }
                Console.WriteLine("Valor inválido.");
            }

            UsuarioRepository.Inserir(novo);
            Console.WriteLine("Usuário cadastrado com sucesso!");
        }

        public static Usuario? FazerLogin(string email, string senha)
        {
            var usuario = UsuarioRepository.BuscarPorEmail(email);
            if (usuario != null && BCrypt.Net.BCrypt.Verify(senha, usuario.Senha))
                return usuario;
            return null;
        }
    }
}

