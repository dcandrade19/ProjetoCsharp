using SistemaSapatosBase.DataBase;
using SistemaSapatosBase.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace SistemaSapatos.ViewModel
{
    public class ModeloViewModel
    {
        private ModeloContexto modeloContexto;

        public ObservableCollection<Modelo> Modelos { get; }

        public String Mensagem { get; set; }

        public Modelo ModeloSelecionado { get; set; } = new Modelo();

        public Estoque EstoqueSelecionado { get; set; }

        public int QuantidadeSelecionada { get; set; }

        public ObservableCollection<Venda> PreVendas { get; set; } = new ObservableCollection<Venda>();

        public ObservableCollection<int> QuantidadesDisponiveis { get; set; } = new ObservableCollection<int>();

        /// <summary>
        /// Metodo que carrega os modelos(sapatos) do banco, e salva
        /// em uma coleção Observable para carregar na grid.
        /// </summary>
        public ModeloViewModel()
        {
            modeloContexto = new ModeloContexto();
            Modelos = new ObservableCollection<Modelo>();

            Modelos = modeloContexto.Carregar();
        }

        public void CarregarComando()
        {
            modeloContexto.Carregar();
        }
        /// <summary>
        /// Função que retorna mensagem se conseguiu ou não salvar o modelo no contexto.
        /// </summary>
        public void SalvarComando(bool messageBoxOff = false)
        {
            if(!messageBoxOff)
            {
                if (!ModeloSelecionado.HasErrors)
                {
                    var id = modeloContexto.Salvar(ModeloSelecionado);
                    if (id > 0)
                    {
                        MessageBox.Show("O modelo ID: " + id + " foi salvo com sucesso.",
                        ("Modelo salvo!"), MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                    else
                    {
                        MessageBox.Show("Ocorreu um erro ao tentar salvar o modelo.",
                        "Não foi possivel salvar o modelo!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Verifique se todos os campos foram preenchidos corretamente.",
                        "Não foi possivel salvar o modelo!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }else
            {
                modeloContexto.Salvar(ModeloSelecionado);
            }          
        }
        
        public void DeletarComando(bool messageBoxOff = false)
        {
            if (!messageBoxOff)
            {
                if (ModeloSelecionado.IdModelo > 0)
                {

                    var id = modeloContexto.Deletar(ModeloSelecionado);
                    if (id > 0)
                    {
                        MessageBox.Show("O modelo ID: " + id + " foi deletado com sucesso.",
                        ("Modelo deletado!"), MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                    else
                    {
                        MessageBox.Show("Ocorreu um erro ao tentar deletar o modelo.",
                        "Não foi possivel deletar o modelo!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            } else
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
