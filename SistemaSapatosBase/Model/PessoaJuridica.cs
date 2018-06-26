using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSapatosBase.Model
{
    public class PessoaJuridica : Pessoa
    {

        private String _razaoSocial;
        private String _cnpj;
        private Endereco _endereco = new Endereco();

        public String RazaoSocial
        {
            get { return _razaoSocial; }
            set { _razaoSocial = value;Notificacao(); }
        }

        public String Cnpj
        {
            get { return _cnpj; }
            set { _cnpj = value;Notificacao(); }
        }

        public Endereco Endereco
        {
            get { return _endereco; }
            set { _endereco = value;Notificacao(); }
        }
    }
}
