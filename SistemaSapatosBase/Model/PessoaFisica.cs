using SistemaSapatos.Base.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSapatosBase.Model
{
    /// <summary>
    /// Representa um cliente do tipo pessoa fisica
    /// </summary>
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

        public override DateTime? DataNascimentoAb
        {
            get { return DataNascimento; }
        }

        public override string CpfCnpj
        {
            get { return Cpf; }
        }

        public override string RazaoSocialAb
        {
            get { return ""; }
        }

        public override Endereco EnderecoAb {
            get { return new Endereco(); }
        }
    }
}
