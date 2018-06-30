using SistemaSapatos.Base.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSapatosBase.Model
{
    public class PessoaFisica : Pessoa
    {
        private String _cpf;
        private DateTime? _dataNascimento;

        public PessoaFisica()
        {
            Nome = string.Empty;
        }

        
        public String Cpf
        {
            get { return _cpf; }
            set { _cpf = value; Notificacao(); ValidateModelProperty(value); }
        }

        public DateTime? DataNascimento
        {
            get { return _dataNascimento; }
            set { _dataNascimento = value; Notificacao(); }
        }

    }
}
