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
    /// <summary>
    /// ViewModel de cliente, realiza as ligações entre as views e o contexto de cliente
    /// </summary>
    public class PessoaViewModel : EntidadeBase
    {
        private PessoaContexto pessoaContexto;

        private ObservableCollection<Pessoa> _clientes;
        /// <summary>
        /// Representa a coleção de clientes/pessoas cadastradas
        /// </summary>
        public ObservableCollection<Pessoa> Clientes
        {
            get { return _clientes; }
            set { _clientes = value; Notificacao(); }
        }
        /// <summary>
        /// Representa a compra atualmente selecionada
        /// </summary>
        public Venda CompraSelecionada { get; set; }

        private string _msgResultadoBusca;

        public string MsgResultadoBusca
        {
            get { return _msgResultadoBusca; }
            set { _msgResultadoBusca = value; Notificacao(); }
        }

        private string _strBusca = string.Empty;
        /// <summary>
        /// Representa o valor a ser localizado em buscas
        /// </summary>
        public string StrBusca
        {
            get { return _strBusca; }
            set { _strBusca = value; Notificacao(); BuscarCliente(value); }
        }

        private bool _isPessoaFisica = true;

        private bool _IsPessoaJuridica = false;
        /// <summary>
        /// Representa uma confirmação do tipo de cliente/pessoa
        /// </summary>
        public bool IsPessoaFisica
        {
            get { return _isPessoaFisica; }
            set { _isPessoaFisica = value; IsPessoaJuridica = !value; Notificacao(); }
        }
        /// <summary>
        /// Representa uma confirmação do tipo de cliente/pessoa
        /// </summary>
        public bool IsPessoaJuridica
        {
            get { return _IsPessoaJuridica; }
            set { _IsPessoaJuridica = value; Notificacao(); }
        }

        private string _cpfCnpjBusca = string.Empty;
        /// <summary>
        /// Representa o Cpf ou Cnpj usado para buscas
        /// </summary>
        [Required(ErrorMessage = "Informe o Cpf/Cnpj do cliente!", AllowEmptyStrings = false)]
        public string CpfCnpjBusca
        {
            get { return _cpfCnpjBusca; }
            set { _cpfCnpjBusca = value; Notificacao(); ValidateModelProperty(value); }
        }

        private Pessoa _clienteSelecionado;
        /// <summary>
        /// Representa o cliente atualmente selecionado
        /// </summary>
        public Pessoa ClienteSelecionado
        {
            get { return _clienteSelecionado; }
            set { _clienteSelecionado = value; Notificacao(); }
        }
        /// <summary>
        /// Construtor que cria e carrega os clientes do banco de dados
        /// </summary>
        public PessoaViewModel()
        {
            pessoaContexto = new PessoaContexto();
            Clientes = new ObservableCollection<Pessoa>();

            Clientes = pessoaContexto.Carregar();
        }
        /// <summary>
        /// Realiza as confirmações para salvar um cliente em banco e chama o metodo responsavel para salvar
        /// </summary>
        /// <param name="messageBoxOff">Define se as messagebox devem ser exibidas</param>
        public void SalvarComando(bool messageBoxOff = false)
        {
            ModeloViewModel modeloViewModel = new ModeloViewModel();
            foreach (Venda venda in ClienteSelecionado.Compras)
            {
                venda.Modelo = null;
            }
            if (ClienteSelecionado.GetType() == typeof(PessoaFisica))
            {
                PessoaFisica pessoaFisica = new PessoaFisica();
                pessoaFisica = (PessoaFisica)ClienteSelecionado;
                if (pessoaFisica.Cpf == null)
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
                    }
                    else
                    {
                        pessoaContexto.Salvar(pessoaFisica);
                    }
                }
                else
                {
                    MessageBox.Show("Verifique se todos os campos foram preenchidos corretamente.",
                        "Não foi possivel salvar o cliente!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
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
                    }
                    else
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
        /// <summary>
        /// Realiza as verificações via messagebox e chama o metodo responsável para deletar um cliente/pessoa
        /// </summary>
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
        /// <summary>
        /// Chama o metodo do contexto responsável por carregar os clientes
        /// </summary>
        public void CarregarComando()
        {
            pessoaContexto.Carregar();
        }
        /// <summary>
        /// Chama o metodo do contexto responsável por buscar o cliente via id
        /// </summary>
        /// <param name="id">Id do cliente a ser localizado</param>
        /// <returns></returns>
        public Pessoa BuscarComando(int id)
        {

            return pessoaContexto.BuscarId(id);
        }
        /// <summary>
        /// Define o tipo do cliente selecionado
        /// </summary>
        public void DefinirTipo()
        {
            if (IsPessoaFisica)
            {
                ClienteSelecionado = new PessoaFisica();
            }
            else
            {
                ClienteSelecionado = new PessoaJuridica();
            }
        }
        /// <summary>
        /// Busca um cliente através do Cpf ou Cnpj
        /// </summary>
        public void BuscarClienteComando()
        {
            List<PessoaFisica> pessoasFisicas;
            List<PessoaJuridica> pessoasJuridicas;
            if (IsPessoaFisica)
            {
                pessoasFisicas = new List<PessoaFisica>();
                foreach (Pessoa p in Clientes)
                {
                    if (p.GetType() == typeof(PessoaFisica))
                    {
                        pessoasFisicas.Add((PessoaFisica)p);
                    }
                }
                var Encontrado = (from cliente in pessoasFisicas where cliente.Cpf == CpfCnpjBusca select cliente).FirstOrDefault();
                if (Encontrado != null)
                {
                    ClienteSelecionado = Encontrado;
                    
                }
                else
                {
                    MessageBox.Show("Não foi localizado o cliente com o CPF : " + CpfCnpjBusca, "Cliente não encontrado", MessageBoxButton.OK,MessageBoxImage.Information);
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
                    MessageBox.Show("Não foi localizado o cliente com o CNPJ : " + CpfCnpjBusca, "Cliente não encontrado", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClienteSelecionado = new PessoaJuridica()
                    {
                        Cnpj = CpfCnpjBusca
                    };
                }
            }
        }
        /// <summary>
        /// Busca os clientes cadastrados
        /// </summary>
        /// <param name="strBuscar">Valor a ser localizado dentre os clientes</param>
        public void BuscarCliente(string strBuscar)
        {
            if (!string.IsNullOrEmpty(strBuscar))
            {
                var resultadoBusca = new ObservableCollection<Pessoa>(Clientes
            .Where(a => a.Nome.Contains(strBuscar) ||
            a.CpfCnpj.Contains(strBuscar) ||
            a.DataNascimentoAb.ToString().Contains(strBuscar) ||
            a.RazaoSocialAb.Contains(strBuscar) || 
            a.EnderecoAb.Rua.Contains(strBuscar) ||
            a.EnderecoAb.Estado.Contains(strBuscar) ||
            a.EnderecoAb.Cidade.Contains(strBuscar) ||
            a.EnderecoAb.Bairro.Contains(strBuscar) ||
            a.EnderecoAb.Numero.ToString().Contains(StrBusca)));
                if (resultadoBusca.Count > 0)
                {

                    MsgResultadoBusca = string.Format("Foram localizados {0} clientes!", resultadoBusca.Count);
                    Clientes = resultadoBusca;
                }
                else
                {           
                    MsgResultadoBusca = "A busca não retornou nenhum resultado!";
                    Clientes = pessoaContexto.Carregar();
                }
                //Clientes = resultadoBusca;
            }
            else
            {
                Clientes = pessoaContexto.Carregar();
                MsgResultadoBusca = string.Empty;
            }
        }
    }
}
