using Lojadegames.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lojadegames.Repositories
{
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

