using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lojadegames.Repositories
{
    public static class Utils
    {
        public static string LerSenhaOculta()
        {
            string senha = "";
            ConsoleKeyInfo tecla;

            do
            {
                tecla = Console.ReadKey(true); // true: não exibe o caractere

                if (tecla.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (tecla.Key == ConsoleKey.Backspace && senha.Length > 0)
                {
                    senha = senha.Substring(0, senha.Length - 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(tecla.KeyChar))
                {
                    senha += tecla.KeyChar;
                    Console.Write("*"); // mostra asterisco para cada caractere
                }

            } while (true);

            return senha;
        }
    }
}
