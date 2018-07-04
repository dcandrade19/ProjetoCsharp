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
    /// <summary>
    /// ViewModel de venda, realiza as ligações entre as views e o contexto de venda
    /// </summary>
    class VendaViewModel : EntidadeBase
    {
        private VendaContexto vendaContexto;

        private ObservableCollection<Venda> _vendas;

        public ObservableCollection<Venda> Vendas
        {
            get { return _vendas; }
            set { _vendas = value; Notificacao(); }
        }

        public String Mensagem { get; set; }

        private string _msgResultadoBusca;

        public string MsgResultadoBusca
        {
            get { return _msgResultadoBusca; }
            set { _msgResultadoBusca = value;Notificacao(); }
        }

        public Venda VendaSelecionada { get; set; }

        private string _strBusca = string.Empty;

        public string StrBusca
        {
            get { return _strBusca; }
            set { _strBusca = value; Notificacao(); BuscarVenda(value); }
        }

        public VendaViewModel()
        {
            vendaContexto = new VendaContexto();
            Vendas = new ObservableCollection<Venda>();

            Vendas = vendaContexto.Carregar();
        }

        public void SalvarComando(Venda venda)
        {
            Mensagem = vendaContexto.Salvar(venda) > 0 ? venda.IdModelo + " Salva com sucesso!" : "Não foi possivel salvar a venda!";
        }

        public void DeletarComando(Venda venda)
        {
            string itemDeletado = vendaContexto.Deletar(venda);

            Mensagem = itemDeletado != null ? itemDeletado + " Deletada com sucesso!" : "Não foi possivel deletar a venda!";
        }

        public Venda BuscarComando(int id)
        {

            return vendaContexto.BuscarId(id);
        }
        /// <summary>
        /// Chama o metodo do contexto para carregar as vendas
        /// </summary>
        public void CarregarComando()
        {

            vendaContexto.Carregar();
        }
       
        /// <summary>
        /// Busca os itens de venda cadastrados
        /// </summary>
        /// <param name="strBuscar">Valor a ser localizado dentre as vendas</param>
        public void BuscarVenda(string strBuscar)
        {
            if (!string.IsNullOrEmpty(strBuscar))
            {
                var resultadoBusca = new ObservableCollection<Venda>(Vendas
            .Where(a => a.Modelo.Nome.Contains(strBuscar) ||
            a.Cliente.Nome.Contains(strBuscar) ||
            a.Preco.ToString().Contains(strBuscar) ||
            a.Total.ToString().Contains(strBuscar) ||
            a.DataVenda.ToString().Contains(strBuscar)));
                if (resultadoBusca.Count > 0)
                {
                    
                    MsgResultadoBusca = string.Format("Foram localizadas {0} vendas!",resultadoBusca.Count);
                    Vendas = resultadoBusca;
                }
                else
                {            
                    MsgResultadoBusca = "A busca não retornou nenhum resultado!";
                    Vendas = vendaContexto.Carregar();
                }
                // Vendas = resultadoBusca;
            }
            else
            {
                Vendas = vendaContexto.Carregar();
                MsgResultadoBusca = string.Empty;  
            }
        }
    }
}
