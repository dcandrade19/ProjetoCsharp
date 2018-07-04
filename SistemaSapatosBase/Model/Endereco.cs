using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSapatosBase.Model
{
    /// <summary>
    /// Representa um endereço
    /// </summary>
    public class Endereco
    {
        public int IdEndereco { get; set; }
        public String Rua { get; set; } = "";
        public int Numero { get; set; } = 0;
        public String Bairro { get; set; } = "";
        public String Cidade { get; set; } = "";
        public String Estado { get; set; } = "";
        public String Complemento { get; set; } = "";
    }
}
