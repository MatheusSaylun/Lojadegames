using Lojadegames.Models;
using Lojadegames.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
