// Estrutura do projeto organizada em camadas:
// - Program.cs (apresentação)
// - Services/UsuarioService.cs (regras de negócio)
// - Repositories/UsuarioRepository.cs (acesso ao banco)
// - Models/Usuario.cs (dados)

// ===================== Program.cs =====================

using Lojadegames.Models;
using Lojadegames.Models.Lojadegames.Models;
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
        Console.WriteLine("[0] Sair do sistema");
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
        public class Cliente
        {
            public int Codigo { get; set; }
            public string Nome { get; set; }
            public string Rg { get; set; }
            public string Cpf { get; set; }
            public string Endereco { get; set; }
            public string Telefone { get; set; }
            public string Email { get; set; }
            public DateTime DataCadastro { get; set; }

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
    public enum CategoriaProduto
    {
        Jogos,
        Acessorios,
        ProdutosGeek
    }
    public class Produto
    {
        public int Id { get; set; }
        public string CodigoBarras { get; set; }
        public string Nome { get; set; }
        public CategoriaProduto Categoria { get; set; }
        public string Fabricante { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }

        // Só para Jogos e Acessórios
        public string? Plataforma { get; set; }
        public int? PrazoGarantiaMeses { get; set; }
    }
    namespace Lojadegames.Models
    {
        public enum FormaPagamento { Dinheiro, Cartao }
        public enum StatusPagamento { Pendente, Pago }
        public enum StatusVenda { EmAndamento, Finalizada }

        public class ItemVenda
        {
            public Produto Produto { get; set; }
            public int Quantidade { get; set; }
            public decimal Subtotal => Produto.Valor * Quantidade;
        }

        public class Venda
        {
            public string Codigo { get; set; }
            public Usuario Cliente { get; set; }
            public List<ItemVenda> Itens { get; set; } = new();
            public DateTime DataVenda { get; set; } = DateTime.Now;
            public FormaPagamento FormaPagamento { get; set; }
            public StatusPagamento StatusPagamento { get; set; } = StatusPagamento.Pendente;
            public StatusVenda StatusVenda { get; set; } = StatusVenda.EmAndamento;

            public decimal ValorTotal => Itens.Sum(i => i.Subtotal);
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
        public static class ClienteRepository
        {
            static string caminhoDb = "usuarios.db";

            public static void CriarTabelaSeNaoExistir()
            {
                using var conexao = new SqliteConnection($"Data Source={caminhoDb}");
                conexao.Open();

                var comando = conexao.CreateCommand();
                comando.CommandText = @"
                CREATE TABLE IF NOT EXISTS Clientes (
                    Codigo INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome TEXT,
                    Rg TEXT,
                    Cpf TEXT,
                    Endereco TEXT,
                    Telefone TEXT,
                    Email TEXT,
                    DataCadastro TEXT
                );";
                comando.ExecuteNonQuery();
            }

            public static void Inserir(Cliente c)
            {
                using var conexao = new SqliteConnection($"Data Source={caminhoDb}");
                conexao.Open();

                var comando = conexao.CreateCommand();
                comando.CommandText = @"
                INSERT INTO Clientes 
                (Nome, Rg, Cpf, Endereco, Telefone, Email, DataCadastro)
                VALUES 
                ($nome, $rg, $cpf, $endereco, $telefone, $email, $datacadastro);";

                comando.Parameters.AddWithValue("$nome", c.Nome);
                comando.Parameters.AddWithValue("$rg", c.Rg);
                comando.Parameters.AddWithValue("$cpf", c.Cpf);
                comando.Parameters.AddWithValue("$endereco", c.Endereco);
                comando.Parameters.AddWithValue("$telefone", c.Telefone);
                comando.Parameters.AddWithValue("$email", c.Email);
                comando.Parameters.AddWithValue("$datacadastro", c.DataCadastro.ToString("yyyy-MM-dd"));

                comando.ExecuteNonQuery();
            }
        }
    }
    public static class ProdutoRepository
    {
        static string caminhoDb = "usuarios.db";

        public static void CriarTabelaSeNaoExistir()
        {
            using var conexao = new SqliteConnection($"Data Source={caminhoDb}");
            conexao.Open();

            var comando = conexao.CreateCommand();
            comando.CommandText = @"
                CREATE TABLE IF NOT EXISTS Produtos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CodigoBarras TEXT,
                    Nome TEXT,
                    Categoria INTEGER,
                    Fabricante TEXT,
                    Quantidade INTEGER,
                    Valor REAL,
                    Plataforma TEXT,
                    PrazoGarantiaMeses INTEGER
                );";
            comando.ExecuteNonQuery();
        }

        public static void Inserir(Produto p)
        {
            using var conexao = new SqliteConnection($"Data Source={caminhoDb}");
            conexao.Open();

            var comando = conexao.CreateCommand();
            comando.CommandText = @"
                INSERT INTO Produtos 
                (CodigoBarras, Nome, Categoria, Fabricante, Quantidade, Valor, Plataforma, PrazoGarantiaMeses)
                VALUES ($codigo, $nome, $categoria, $fabricante, $quantidade, $valor, $plataforma, $prazo);";

            comando.Parameters.AddWithValue("$codigo", p.CodigoBarras);
            comando.Parameters.AddWithValue("$nome", p.Nome);
            comando.Parameters.AddWithValue("$categoria", (int)p.Categoria);
            comando.Parameters.AddWithValue("$fabricante", p.Fabricante);
            comando.Parameters.AddWithValue("$quantidade", p.Quantidade);
            comando.Parameters.AddWithValue("$valor", p.Valor);
            comando.Parameters.AddWithValue("$plataforma", p.Plataforma ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("$prazo", p.PrazoGarantiaMeses ?? (object)DBNull.Value);

            comando.ExecuteNonQuery();
        }
        public static Produto? BuscarPorCodigoBarras(string codigo)
        {
            using var conexao = new SqliteConnection($"Data Source={caminhoDb}");
            conexao.Open();

            var comando = conexao.CreateCommand();
            comando.CommandText = "SELECT * FROM Produtos WHERE CodigoBarras = $codigo";
            comando.Parameters.AddWithValue("$codigo", codigo);

            using var reader = comando.ExecuteReader();
            if (reader.Read())
            {
                return new Produto
                {
                    Id = reader.GetInt32(0),
                    CodigoBarras = reader.GetString(1),
                    Nome = reader.GetString(2),
                    Categoria = (CategoriaProduto)reader.GetInt32(3),
                    Fabricante = reader.GetString(4),
                    Quantidade = reader.GetInt32(5),
                    Valor = reader.GetDecimal(6),
                    Plataforma = reader.IsDBNull(7) ? null : reader.GetString(7),
                    PrazoGarantiaMeses = reader.IsDBNull(8) ? null : reader.GetInt32(8)
                };
            }
            return null;
        }

    }
    public static class VendaRepository
    {
        static string caminhoDb = "usuarios.db";

        public static void CriarTabelaSeNaoExistir()
        {
            using var conexao = new SqliteConnection($"Data Source={caminhoDb}");
            conexao.Open();

            var comando = conexao.CreateCommand();
            comando.CommandText = @"
                CREATE TABLE IF NOT EXISTS Vendas (
                    Codigo TEXT PRIMARY KEY,
                    EmailCliente TEXT,
                    Data TEXT,
                    ValorTotal REAL,
                    FormaPagamento INTEGER,
                    StatusPagamento INTEGER,
                    StatusVenda INTEGER
                );

                CREATE TABLE IF NOT EXISTS ItensVenda (
                    CodigoVenda TEXT,
                    CodigoBarras TEXT,
                    Nome TEXT,
                    Quantidade INTEGER,
                    ValorUnitario REAL
                );
            ";
            comando.ExecuteNonQuery();
        }

        public static void SalvarVenda(Venda venda)
        {
            using var conexao = new SqliteConnection($"Data Source={caminhoDb}");
            conexao.Open();

            var transacao = conexao.BeginTransaction();

            try
            {
                var cmdVenda = conexao.CreateCommand();
                cmdVenda.CommandText = @"
                    INSERT INTO Vendas (Codigo, EmailCliente, Data, ValorTotal, FormaPagamento, StatusPagamento, StatusVenda)
                    VALUES ($codigo, $cliente, $data, $valor, $forma, $statusPag, $statusVenda);
                ";
                cmdVenda.Parameters.AddWithValue("$codigo", venda.Codigo);
                cmdVenda.Parameters.AddWithValue("$cliente", venda.Cliente.Email);
                cmdVenda.Parameters.AddWithValue("$data", venda.DataVenda.ToString("yyyy-MM-dd HH:mm:ss"));
                cmdVenda.Parameters.AddWithValue("$valor", venda.ValorTotal);
                cmdVenda.Parameters.AddWithValue("$forma", (int)venda.FormaPagamento);
                cmdVenda.Parameters.AddWithValue("$statusPag", (int)venda.StatusPagamento);
                cmdVenda.Parameters.AddWithValue("$statusVenda", (int)venda.StatusVenda);
                cmdVenda.ExecuteNonQuery();

                foreach (var item in venda.Itens)
                {
                    var cmdItem = conexao.CreateCommand();
                    cmdItem.CommandText = @"
                        INSERT INTO ItensVenda (CodigoVenda, CodigoBarras, Nome, Quantidade, ValorUnitario)
                        VALUES ($codigoVenda, $codigoBarras, $nome, $quantidade, $valorUnitario);
                    ";
                    cmdItem.Parameters.AddWithValue("$codigoVenda", venda.Codigo);
                    cmdItem.Parameters.AddWithValue("$codigoBarras", item.Produto.CodigoBarras);
                    cmdItem.Parameters.AddWithValue("$nome", item.Produto.Nome);
                    cmdItem.Parameters.AddWithValue("$quantidade", item.Quantidade);
                    cmdItem.Parameters.AddWithValue("$valorUnitario", item.Produto.Valor);
                    cmdItem.ExecuteNonQuery();
                }

                transacao.Commit();
            }
            catch
            {
                transacao.Rollback();
                throw;
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
    public static class ProdutoService
    {
        static ProdutoService()
        {
            ProdutoRepository.CriarTabelaSeNaoExistir();
        }

        public static void CadastrarProduto()
        {
            Produto p = new Produto();

            Console.Write("Código de Barras: ");
            p.CodigoBarras = Console.ReadLine();

            Console.Write("Nome do Produto: ");
            p.Nome = Console.ReadLine();

            Console.Write("Fabricante: ");
            p.Fabricante = Console.ReadLine();

            Console.Write("Quantidade: ");
            p.Quantidade = int.Parse(Console.ReadLine());

            Console.Write("Valor (R$): ");
            p.Valor = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Categoria [0] Jogos, [1] Acessorios, [2] ProdutosGeek: ");
            p.Categoria = (CategoriaProduto)int.Parse(Console.ReadLine());

            if (p.Categoria == CategoriaProduto.Jogos || p.Categoria == CategoriaProduto.Acessorios)
            {
                Console.Write("Plataforma: ");
                p.Plataforma = Console.ReadLine();

                Console.Write("Prazo de Garantia (meses): ");
                p.PrazoGarantiaMeses = int.Parse(Console.ReadLine());
            }

            ProdutoRepository.Inserir(p);
            Console.WriteLine("Produto cadastrado com sucesso!");
        }
    }
    public static class ClienteService
    {
        static ClienteService()
        {
            ClienteRepository.CriarTabelaSeNaoExistir();
        }

        public static void CadastrarCliente()
        {
            Cliente cliente = new Cliente();

            Console.Write("Nome: ");
            cliente.Nome = Console.ReadLine();

            Console.Write("RG: ");
            cliente.Rg = Console.ReadLine();

            do
            {
                Console.Write("CPF: ");
                cliente.Cpf = Console.ReadLine();
                if (!cliente.CpfValido())
                    Console.WriteLine("CPF inválido!");
            } while (!cliente.CpfValido());

            Console.Write("Endereço: ");
            cliente.Endereco = Console.ReadLine();

            do
            {
                Console.Write("Telefone: ");
                cliente.Telefone = Console.ReadLine();
                if (!cliente.TelefoneValido())
                    Console.WriteLine("Telefone inválido!");
            } while (!cliente.TelefoneValido());

            do
            {
                Console.Write("Email: ");
                cliente.Email = Console.ReadLine();
                if (!cliente.EmailValido())
                    Console.WriteLine("Email inválido!");
            } while (!cliente.EmailValido());

            cliente.DataCadastro = DateTime.Now;

            ClienteRepository.Inserir(cliente);
            Console.WriteLine("Cliente cadastrado com sucesso!");
        }
    }
    public static class VendaService
    {
        static VendaService()
        {
            VendaRepository.CriarTabelaSeNaoExistir();
        }

        public static void RealizarVenda(Usuario cliente)
        {
            var venda = new Venda
            {
                Cliente = cliente,
                Codigo = Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
            };

            while (true)
            {
                Console.Write("Código de barras do produto (ou 'fim'): ");
                var cod = Console.ReadLine();
                if (cod.ToLower() == "fim") break;

                // Buscar produto no repositório (pode implementar essa função depois)
                var produto = ProdutoRepository.BuscarPorCodigoBarras(cod);
                if (produto == null)
                {
                    Console.WriteLine("Produto não encontrado.");
                    continue;
                }

                Console.Write("Quantidade: ");
                int qtd = int.Parse(Console.ReadLine());

                venda.Itens.Add(new ItemVenda { Produto = produto, Quantidade = qtd });
            }

            Console.WriteLine("Resumo da venda:");
            foreach (var item in venda.Itens)
                Console.WriteLine($"{item.Produto.Nome} - {item.Quantidade} x R${item.Produto.Valor:F2} = R${item.Subtotal:F2}");

            Console.WriteLine($"Total: R${venda.ValorTotal:F2}");

            Console.Write("Forma de pagamento (0=Dinheiro, 1=Cartão): ");
            venda.FormaPagamento = (FormaPagamento)int.Parse(Console.ReadLine());

            venda.StatusPagamento = StatusPagamento.Pago;
            venda.StatusVenda = StatusVenda.Finalizada;

            VendaRepository.SalvarVenda(venda);

            Console.WriteLine("Venda registrada com sucesso!");
        }

        public static bool AutorizarSupervisor()
        {
            Console.WriteLine("Email do supervisor: ");
            var email = Console.ReadLine();

            Console.WriteLine("Senha: ");
            var senha = UsuarioRepository.Utils.LerSenhaOculta();

            var usuario = UsuarioService.FazerLogin(email, senha);

            return usuario != null && usuario.Ehsupervisor;
        }
    }
}

