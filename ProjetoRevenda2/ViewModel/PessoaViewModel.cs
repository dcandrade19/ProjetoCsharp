using SistemaSapatosBase.DataBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SistemaSapatosBase.Model;
using SistemaSapatos.Base.Base;

namespace SistemaSapatos.ViewModel
{
    class PessoaViewModel : EntidadeBase
    {
        private PessoaContexto pessoaContexto;

        public ObservableCollection<Pessoa> Clientes { get; }

        public String Mensagem { get; set; }

        public Venda CompraSelecionada { get; set; }

        private bool _isPessoaFisica = true;

        private bool _IsPessoaJuridica = false;

        public bool IsPessoaFisica
        {
            get { return _isPessoaFisica; }
            set { _isPessoaFisica = value;  IsPessoaJuridica = !value; Notificacao(); }
        }

        public bool IsPessoaJuridica
        {
            get { return _IsPessoaJuridica; }
            set { _IsPessoaJuridica = value;  Notificacao(); }
        }

        private string _cpfCnpjBusca;

        public string CpfCnpjBusca
        {
            get { return _cpfCnpjBusca; }
            set { _cpfCnpjBusca = value; Notificacao(); }
        }

        private Pessoa _clienteSelecionado;

        public Pessoa ClienteSelecionado
        {
            get { return _clienteSelecionado; }
            set { _clienteSelecionado = value; Notificacao(); }
        }
        public PessoaViewModel()
        {
            pessoaContexto = new PessoaContexto();
            Clientes = new ObservableCollection<Pessoa>();
            
            Clientes = pessoaContexto.Carregar();
        }

        public void SalvarComando()
        {
            ModeloViewModel modeloViewModel = new ModeloViewModel();
            foreach(Venda venda in ClienteSelecionado.Compras)
            {
                venda.Modelo = null;
            }
            if (ClienteSelecionado.GetType() == typeof(PessoaFisica))
            {
                PessoaFisica pessoaFisica = new PessoaFisica();
                pessoaFisica = (PessoaFisica)ClienteSelecionado;
                if(pessoaFisica.Cpf == null)
                {
                    pessoaFisica.Cpf = CpfCnpjBusca;
                }
                pessoaContexto.Salvar(pessoaFisica);
            } else
            {
                PessoaJuridica pessoaJuridica = new PessoaJuridica();
                pessoaJuridica = (PessoaJuridica)ClienteSelecionado;
                if (pessoaJuridica.Cnpj == null)
                {
                    pessoaJuridica.Cnpj = CpfCnpjBusca;
                }
                pessoaContexto.Salvar(pessoaJuridica);
            }
        }

        public void DeletarComando()
        {
            if (ClienteSelecionado.IdPessoa > 0)
            {
                pessoaContexto.Deletar(ClienteSelecionado);
            }
        }

        public Pessoa BuscarComando(int id)
        {

            return pessoaContexto.BuscarId(id);
        }
        public void DefinirTipo()
        {
            if(IsPessoaFisica)
            {
                ClienteSelecionado = new PessoaFisica();      
            } else
            {
                ClienteSelecionado = new PessoaJuridica();
            }
        }
        public void BuscarClienteComando()
        {
            List<PessoaFisica> pessoasFisicas;
            List<PessoaJuridica> pessoasJuridicas;
            if (IsPessoaFisica)
            {
                pessoasFisicas = new List<PessoaFisica>();
                foreach(Pessoa p in Clientes)
                {
                    if(p.GetType() == typeof(PessoaFisica))
                    {
                        pessoasFisicas.Add((PessoaFisica)p);
                    }
                }
               var Encontrado = (from cliente in pessoasFisicas where cliente.Cpf == CpfCnpjBusca select cliente).FirstOrDefault();
               if(Encontrado != null)
                {
                    ClienteSelecionado = Encontrado;
                } else
                {

                    ClienteSelecionado = new PessoaFisica()
                    {
                        Cpf = CpfCnpjBusca
                    };
                }
            }
            else
            {
                pessoasJuridicas = new List<PessoaJuridica>();
                foreach (Pessoa p in Clientes)
                {
                    if (p.GetType() == typeof(PessoaJuridica))
                    {
                        pessoasJuridicas.Add((PessoaJuridica)p);
                    }
                }
                var Encontrado = (from cliente in pessoasJuridicas where cliente.Cnpj == CpfCnpjBusca select cliente).FirstOrDefault();
                if (Encontrado != null)
                {
                    ClienteSelecionado = Encontrado;
                }
                else
                {
                    ClienteSelecionado = new PessoaJuridica()
                    {
                        Cnpj = CpfCnpjBusca
                    };
                }
            }
        }
    }
}
