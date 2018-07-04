using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSapatosBase.Model
{
    /// <summary>
    /// Representa um cliente do tipo pessoa juridica
    /// </summary>
    public class PessoaJuridica : Pessoa
    {

        private String _razaoSocial;
        private String _cnpj;
        private Endereco _endereco = new Endereco();

        public PessoaJuridica()
        {
            Nome = string.Empty;
            RazaoSocial = string.Empty;
        }

        [Required(ErrorMessage = "A razão social da empresa é obrigatória!", AllowEmptyStrings = false)]
        public String RazaoSocial
        {
            get { return _razaoSocial; }
            set { _razaoSocial = value; Notificacao(); ValidateModelProperty(value); }
        }

        public override string RazaoSocialAb
        {
            get { return RazaoSocial; }
        }

        public String Cnpj
        {
            get { return _cnpj; }
            set { _cnpj = value; Notificacao(); ValidateModelProperty(value); }
        }

        public Endereco Endereco
        {
            get { return _endereco; }
            set { _endereco = value; Notificacao(); }
        }

        public override string CpfCnpj
        {
            get { return Cnpj; }
        }

        public override DateTime? DataNascimentoAb
        {
            get { return new DateTime(); }
        }

        public override Endereco EnderecoAb
        {
            get { return Endereco; }
        }
    }
}
