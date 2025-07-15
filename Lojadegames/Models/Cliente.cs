using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lojadegames.Models
{
    public class Cliente
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; }

        public bool EmailValido() =>
            !string.IsNullOrEmpty(Email) &&
            System.Net.Mail.MailAddress.TryCreate(Email, out _);

        public bool CpfValido()
        {
            if (string.IsNullOrWhiteSpace(Cpf)) return false;
            string cpf = new string(Cpf.Where(char.IsDigit).ToArray());
            if (cpf.Length != 11 || cpf.Distinct().Count() == 1) return false;

            int Soma(int[] pesos) => cpf.Take(pesos.Length).Select((c, i) => (c - '0') * pesos[i]).Sum();
            int Resto(int soma) => soma % 11 < 2 ? 0 : 11 - (soma % 11);

            int[] peso1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] peso2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            return Resto(Soma(peso1)) == cpf[9] - '0' && Resto(Soma(peso2)) == cpf[10] - '0';
        }

        public bool TelefoneValido()
        {
            string tel = new string((Telefone ?? "").Where(char.IsDigit).ToArray());
            return tel.Length >= 10 && tel.Length <= 11;
        }
    }
}

