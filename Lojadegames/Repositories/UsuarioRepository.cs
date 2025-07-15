using Lojadegames.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
