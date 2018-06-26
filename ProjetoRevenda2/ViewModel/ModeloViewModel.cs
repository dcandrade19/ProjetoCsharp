using SistemaSapatos.Base.Base;
using SistemaSapatosBase.DataBase;
using SistemaSapatosBase.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SistemaSapatos.ViewModel
{
    class ModeloViewModel
    {
        private ModeloContexto modeloContexto;

        public ObservableCollection<Modelo> Modelos { get; }

        public String Mensagem { get; set; }

        public Modelo ModeloSelecionado { get; set; } = new Modelo();

        public Estoque EstoqueSelecionado { get; set; }

        public int QuantidadeSelecionada { get; set; }

        public ObservableCollection<Venda> PreVendas { get; set; } = new ObservableCollection<Venda>();

        public ObservableCollection<int> QuantidadesDisponiveis { get; set; } = new ObservableCollection<int>();

        //Metodo que carrega os modelos(sapatos) do banco, e salva em uma coleção Observable para carregar na grid.
        public ModeloViewModel()
        {
            modeloContexto = new ModeloContexto();
            Modelos = new ObservableCollection<Modelo>();

            Modelos = modeloContexto.Carregar();
        }

        //Função que retorna mensagem se conseguiu ou não salvar o modelo no contexto.
        public void SalvarComando()
        {
            modeloContexto.Salvar(ModeloSelecionado);
        }

        public void DeletarComando()
        {
            if(ModeloSelecionado.IdModelo > 0)
            {
                modeloContexto.Deletar(ModeloSelecionado);
            }   
        }

        public void EditarEstoquesComando(IList<Estoque> estoques)
        {
            modeloContexto.EditarEstoques(estoques);
        }

        public Modelo BuscarComando(int id)
        {
            return modeloContexto.BuscarId(id);
        }

        public void CalcularQuantidadeDisponivel()
        {
            QuantidadesDisponiveis.Clear();
            if (EstoqueSelecionado != null)
            {
                int x = 1;
                while (x <= EstoqueSelecionado.Quantidade)
                {
                    QuantidadesDisponiveis.Add(x);
                    x++;
                }
            }
        }
    }
}
