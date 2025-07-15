using Lojadegames.Models;
using Lojadegames.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lojadegames.Services
{
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
}
