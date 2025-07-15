using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lojadegames.Models
{
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
}
