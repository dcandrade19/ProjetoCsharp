using SistemaSapatos.ViewModel;
using SistemaSapatosBase.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjetoRevenda2
{

    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Tela Modelos // Variaveis da tela modelos
        // Lista de valores dos combobox de materiais e cores
        private List<string> materiais = new List<string>()
        {
            "Couro",
            "Sintetico",
            "Tecido",
            "Plastico",
            "Camurça",
            "Outro"
        };
        private List<string> cores = new List<string>()
        {
            "Preto",
            "Branco",
            "Azul",
            "Vermelho",
            "Amarelo",
            "Cinza",
            "Outra"
        };
        // ViewModel de Sapatos
        private ModeloViewModel ModeloViewModel { get; set; }
        #endregion
        #region Tela Clientes // Variaveis da tela clientes
        private PessoaViewModel PessoaViewModel { get; set; }
        private VendaViewModel VendaViewModel { get; set; }
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            IniciarComboBoxModelos();
            ModeloViewModel = new ModeloViewModel();
            tabProdutos.DataContext = ModeloViewModel;

            PessoaViewModel = new PessoaViewModel();
            tabClientes.DataContext = PessoaViewModel;
            VendaViewModel = new VendaViewModel();
            DataNovaVenda.DataContext = ModeloViewModel;

            tabListaVendas.DataContext = VendaViewModel;
            tabListaClientes.DataContext = PessoaViewModel;
        }

        #region Tela Modelos // Metodos da tela de modelos
        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            ModeloViewModel.SalvarComando();
            LimparModelo();
        }

        private void BtnLimparModelo_Click(object sender, RoutedEventArgs e)
        {
            LimparModelo();
        }

        private void BtnAddEstoque_Click(object sender, RoutedEventArgs e)
        {
            AdicionarEstoque();
        }

        private void BtnDeletar_Click(object sender, RoutedEventArgs e)
        {
            ModeloViewModel.DeletarComando();
        }

        // Inicializa as ComboBox com seus valores padrão
        private void IniciarComboBoxModelos()
        {
            int i = 0;
            while (i <= 100)
            {
                cbxQuantidades.Items.Add(i);
                if (i >= 30 && i <= 48)
                {
                    cbxTamanhos.Items.Add(i);
                }
                i++;
            }
            cbxMateriais.ItemsSource = materiais;
            cbxCores.ItemsSource = cores;
        }
        // Adiciona ou atualiza um item de estoque na tela de modelos
        private void AdicionarEstoque()
        {
            bool atualizado = false;
            Estoque estoqueAdd = new Estoque()
            {
                Tamanho = (int)cbxTamanhos.SelectedValue,
                Quantidade = (int)cbxQuantidades.SelectedValue
            };
            while (!atualizado)
            {
                foreach (Estoque estoque in ModeloViewModel.ModeloSelecionado.Estoques)
                {
                    if (estoque.Tamanho == estoqueAdd.Tamanho)
                    {
                        estoque.Quantidade += estoqueAdd.Quantidade;
                        atualizado = true;
                    }
                }
                if (!atualizado)
                {
                    ModeloViewModel.ModeloSelecionado.Estoques.Add(estoqueAdd);
                    atualizado = true;
                }
            }
        }
        public void LimparModelo()
        {
            dataModelos.SelectedIndex = -1;
            dataModelos.SelectedItem = new Modelo();     
        }
        #endregion

        #region Tela Clientes // Metodos da tela de clientes
        private void BtnAddVenda_Click(object sender, RoutedEventArgs e)
        {
            //modeloViewModel.PreVendaComando();
            AdicionarVenda();
            cbxModelo.SelectedIndex = -1;
        }

        private void CbxTamanho_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModeloViewModel.CalcularQuantidadeDisponivel();
        }

        private void BtnRemover_Click(object sender, RoutedEventArgs e)
        {
            //modeloViewModel.RemoverPreVendaComando();
            RemoverVenda();
            cbxModelo.SelectedIndex = -1;
        }

        private void AdicionarVenda()
        {
            bool atualizado = false;
            Venda vendaAdd = new Venda()
            {
                IdModelo = ModeloViewModel.ModeloSelecionado.IdModelo,
                Modelo = ModeloViewModel.ModeloSelecionado,
                NomeModeloPreCriacao = ModeloViewModel.ModeloSelecionado.Nome,
                QtdItens = ModeloViewModel.QuantidadeSelecionada,
                Tamanho = ModeloViewModel.EstoqueSelecionado.Tamanho,
                Preco = ModeloViewModel.ModeloSelecionado.Preco,
                Total = ModeloViewModel.ModeloSelecionado.Preco * ModeloViewModel.QuantidadeSelecionada
            };
            while (!atualizado)
            {
                foreach (Venda venda in PessoaViewModel.ClienteSelecionado.Compras)
                {
                    if (venda.Tamanho == vendaAdd.Tamanho && venda.IdVenda == 0 && venda.Modelo == vendaAdd.Modelo)
                    {
                        venda.QtdItens += vendaAdd.QtdItens;
                        atualizado = true;
                    }
                }
                if (!atualizado)
                {
                    PessoaViewModel.ClienteSelecionado.Compras.Add(vendaAdd);
                    atualizado = true;
                }
            }
            ModeloViewModel.EstoqueSelecionado.Quantidade -= ModeloViewModel.QuantidadeSelecionada;
        }

        public void RemoverVenda()
        {
            if(PessoaViewModel.CompraSelecionada.IdVenda == 0)
            {
                foreach (Estoque estoque in PessoaViewModel.CompraSelecionada.Modelo.Estoques)
                {
                    if (estoque.Tamanho == PessoaViewModel.CompraSelecionada.Tamanho)
                    {
                        estoque.Quantidade += PessoaViewModel.CompraSelecionada.QtdItens;
                    }
                }
                PessoaViewModel.ClienteSelecionado.Compras.Remove(PessoaViewModel.CompraSelecionada);
            }    
        }

        private void RbnPessoaFisica_Checked(object sender, RoutedEventArgs e)
        {
            PessoaViewModel.CpfCnpjBusca = "";
            rbnPessoaJuridica.IsChecked = false;
            txbCpfCnpj.Text = "CPF:";
            PessoaViewModel.DefinirTipo();
        }

        private void RbnPessoaJuridica_Checked(object sender, RoutedEventArgs e)
        {
            PessoaViewModel.CpfCnpjBusca = "";
            rbnPessoaFisica.IsChecked = false;
            txbCpfCnpj.Text = "CNPJ:";
            PessoaViewModel.DefinirTipo();
        }

        private void BtnSalvarCliente_Click(object sender, RoutedEventArgs e)
        {
            foreach (Venda venda in PessoaViewModel.ClienteSelecionado.Compras)
            {
                if (venda.IdVenda == 0)
                {
                    ModeloViewModel.ModeloSelecionado = venda.Modelo;
                    ModeloViewModel.SalvarComando();
                }
            }
            PessoaViewModel.SalvarComando();
            VendaViewModel.CarregarComando();
            LimparCliente();
        }

        private void BtnDeletarCliente_Click(object sender, RoutedEventArgs e)
        {
            PessoaViewModel.DeletarComando();
        }

        private void BtnLimparCliente_Click(object sender, RoutedEventArgs e)
        {
            LimparCliente();
        }

        private void BtnBuscarCliente_Click(object sender, RoutedEventArgs e)
        {
            LimparCliente();
            PessoaViewModel.BuscarClienteComando(); 
        }

        public void LimparCliente()
        {
            PessoaViewModel.ClienteSelecionado = new PessoaFisica();
        }

        #endregion

        private void btnGerarXmlVendas_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnGerarXmlClientes_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
