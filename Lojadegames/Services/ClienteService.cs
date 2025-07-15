using Lojadegames.Models;
using Lojadegames.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lojadegames.Services
{
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
}
