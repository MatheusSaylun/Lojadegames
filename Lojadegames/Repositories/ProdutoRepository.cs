using Lojadegames.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lojadegames.Repositories
{
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
}
