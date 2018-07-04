using SistemaSapatosBase.DataBase;
using SistemaSapatosBase.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace SistemaSapatos.ViewModel
{
    /// <summary>
    /// ViewModel de modelos de sapato, realiza as ligações entre as views e o contexto de modelo
    /// </summary>
    public class ModeloViewModel
    {
        private ModeloContexto modeloContexto;
        /// <summary>
        /// Representa a coleção de modelos de sapatos em banco
        /// </summary>
        public ObservableCollection<Modelo> Modelos { get; }
        /// <summary>
        /// Representa o modelo atualmente selecionado
        /// </summary>
        public Modelo ModeloSelecionado { get; set; } = new Modelo();
        /// <summary>
        /// Representa o estoque atualmente selecionado
        /// </summary>
        public Estoque EstoqueSelecionado { get; set; }
        /// <summary>
        /// Representa a quantidade atual selecionada
        /// </summary>
        public int QuantidadeSelecionada { get; set; }

        //public ObservableCollection<Venda> PreVendas { get; set; } = new ObservableCollection<Venda>();

        /// <summary>
        /// Representa a coleção de todas as quantidades disponiveis para o modelo
        /// </summary>
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

        /// <summary>
        /// Chama o metodo do contexto que carrega os modelos 
        /// </summary>
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
        
        /// <summary>
        /// Realiza a confirmação via messagebox e deleta o modelo selecionado
        /// </summary>
        /// <param name="messageBoxOff">Define se os messagebox de confirmação devem ser exibidos</param>
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
        /// <summary>
        /// Chama o metodo de edição de estoque do contexto
        /// </summary>
        /// <param name="estoques"> Lista de estoques para edição</param>
        public void EditarEstoquesComando(IList<Estoque> estoques)
        {
            modeloContexto.EditarEstoques(estoques);
        }

        /// <summary>
        /// Chama o metodo de busca por id do contexto
        /// </summary>
        /// <param name="id"> Id do modelo a deletar</param>
        public Modelo BuscarComando(int id)
        {
            return modeloContexto.BuscarId(id);
        }

        /// <summary>
        /// Calcula a quantidade atual disponivel para o estoque selecionado
        /// </summary>
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
