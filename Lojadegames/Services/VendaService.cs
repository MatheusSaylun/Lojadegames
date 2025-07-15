using Lojadegames.Models;
using Lojadegames.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lojadegames.Services
{
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
            var senha = Utils.LerSenhaOculta();

            var usuario = UsuarioService.FazerLogin(email, senha);

            return usuario != null && usuario.Ehsupervisor;
        }
    }
}

