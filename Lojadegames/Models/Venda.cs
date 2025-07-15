using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lojadegames.Models
{
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
