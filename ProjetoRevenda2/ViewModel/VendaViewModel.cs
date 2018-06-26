using SistemaSapatosBase.DataBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SistemaSapatosBase.Model;

namespace SistemaSapatos.ViewModel
{
    class VendaViewModel
    {
        private VendaContexto vendaContexto;

        public ObservableCollection<Venda> Vendas { get; }

        public String Mensagem { get; set; }

        public Venda VendaSelecionada { get; set; }

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

        public void CarregarComando()
        {

            vendaContexto.Carregar();
        }

    }
}
