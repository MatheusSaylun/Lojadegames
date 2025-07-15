using Lojadegames.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lojadegames.Repositories
{
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
