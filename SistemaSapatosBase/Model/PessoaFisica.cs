using SistemaSapatos.Base.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSapatosBase.Model
{
    public class PessoaFisica : Pessoa
    {
        private String _cpf;
        private DateTime? _dataNascimento;

        public String Cpf
        {
            get { return _cpf; }
            set { _cpf = value; Notificacao(); }
        }

        public DateTime? DataNascimento
        {
            get { return _dataNascimento; }
            set { _dataNascimento = value; Notificacao(); }
        }

    }
}
