using SistemaSapatosBase.DataBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SistemaSapatosBase.Model;
using SistemaSapatos.Base.Base;
using System.Windows;
using System.ComponentModel.DataAnnotations;

namespace SistemaSapatos.ViewModel
{
    public class PessoaViewModel : EntidadeBase
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

        private string _cpfCnpjBusca = string.Empty;

        [Required(ErrorMessage = "Informe o Cpf/Cnpj do cliente!", AllowEmptyStrings = false)]
        public string CpfCnpjBusca
        {
            get { return _cpfCnpjBusca; }
            set { _cpfCnpjBusca = value; Notificacao(); ValidateModelProperty(value); }
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

        public void SalvarComando(bool messageBoxOff = false)
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
                if (!pessoaFisica.HasErrors)
                {
                    var compras = from c in pessoaFisica.Compras
                                  where c.IdVenda == 0
                                  select c;
                    List<Venda> listacompras = compras.ToList();
                    if (!messageBoxOff)
                    {
                        var id = pessoaContexto.Salvar(pessoaFisica);
                        if (id > 0)
                        {
                            MessageBox.Show("O cliente ID: " + id + " foi salvo com sucesso.",
                            ("Cliente salvo!"), MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            if (listacompras.Count > 0)
                            {
                                MessageBox.Show("Foram registradas: " + listacompras.Count + " vendas.",
                            ("Vendas registradas!"), MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ocorreu um erro ao tentar salvar o cliente.",
                            "Não foi possivel salvar o cliente!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    } else
                    {
                        pessoaContexto.Salvar(pessoaFisica);
                    }
                }
                else
                {
                    MessageBox.Show("Verifique se todos os campos foram preenchidos corretamente.",
                        "Não foi possivel salvar o cliente!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            } else
            {
                PessoaJuridica pessoaJuridica = new PessoaJuridica();
                pessoaJuridica = (PessoaJuridica)ClienteSelecionado;
                if (pessoaJuridica.Cnpj == null)
                {
                    pessoaJuridica.Cnpj = CpfCnpjBusca;
                }
                if (!pessoaJuridica.HasErrors)
                {
                    var compras = from c in pessoaJuridica.Compras
                                  where c.IdVenda == 0
                                  select c;
                    List<Venda> listacompras = compras.ToList();
                    if (!messageBoxOff)
                    {
                        var id = pessoaContexto.Salvar(pessoaJuridica);
                        if (id > 0)
                        {
                            MessageBox.Show("O cliente ID: " + id + " foi salvo com sucesso.",
                            ("Cliente salvo!"), MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            if (listacompras.Count > 0)
                            {
                                MessageBox.Show("Foram registradas: " + listacompras.Count + " vendas.",
                            ("Vendas registradas!"), MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ocorreu um erro ao tentar salvar o cliente.",
                            "Não foi possivel salvar o cliente!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    } else
                    {
                        pessoaContexto.Salvar(pessoaJuridica);
                    }
                }
                else
                {
                    MessageBox.Show("Verifique se todos os campos foram preenchidos corretamente.",
                        "Não foi possivel salvar o cliente!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        public void DeletarComando()
        {
            if (ClienteSelecionado.IdPessoa > 0)
            {
                var id = pessoaContexto.Deletar(ClienteSelecionado);
                if (id > 0)
                {
                    MessageBox.Show("O cliente ID: " + id + " foi deletado com sucesso.",
                    ("Cliente deletado!"), MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                {
                    MessageBox.Show("Ocorreu um erro ao tentar deletar o cliente.",
                    "Não foi possivel deletar o cliente!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void CarregarComando()
        {
            pessoaContexto.Carregar();
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
