using ClosedXML.Excel;
using Microsoft.Win32;
using SistemaSapatos.ViewModel;
using SistemaSapatosBase.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        string pathRelatorios;
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
            pathRelatorios = AppDomain.CurrentDomain.BaseDirectory;
            pathRelatorios += "Relatorios";
            if (!Directory.Exists(pathRelatorios))
            {
                Directory.CreateDirectory(pathRelatorios);
                return;
            }
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
            if (PessoaViewModel.CompraSelecionada.IdVenda == 0)
            {
                foreach (Estoque estoque in PessoaViewModel.CompraSelecionada.Modelo.Estoques)
                {
                    if (estoque.Tamanho == PessoaViewModel.CompraSelecionada.Tamanho)
                    {
                        estoque.Quantidade += PessoaViewModel.CompraSelecionada.QtdItens;
                    }
                }
                PessoaViewModel.ClienteSelecionado.Compras.Remove(PessoaViewModel.CompraSelecionada);
                cbxQuantidade.Items.Refresh();
            }
        }

        public void RemoverVendasLotes()
        {
            var compras = PessoaViewModel.ClienteSelecionado.Compras.ToList();
            foreach (Venda venda in compras)
            {
                PessoaViewModel.CompraSelecionada = venda;
                RemoverVenda();
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
            RemoverVendasLotes();
            PessoaViewModel.DeletarComando();
            LimparCliente();
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
            RemoverVendasLotes();
            PessoaViewModel.ClienteSelecionado = new PessoaFisica();
        }

        #endregion


        private void BtnGerarXmlVendas_Click(object sender, RoutedEventArgs e)
        {
            GerarXmlVendas();
        }

        private void BtnGerarXmlClientes_Click(object sender, RoutedEventArgs e)
        {
            GerarXmlClientes();
        }
        // Relatorio de vendas
        public void GerarXmlVendas()
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Relatorio de Vendas");
            int x = 3;
            worksheet.Cell("A1").Value = "Relatorio de Vendas - Gerado em " + String.Format("{0:dd/MM/yyyy} as {0:HH:mm:ss}", DateTime.Now);
            worksheet.Cell("A2").Value = "#";
            worksheet.Cell("B2").Value = "Modelo";
            worksheet.Cell("C2").Value = "Tamanho";
            worksheet.Cell("D2").Value = "Quantidade";
            worksheet.Cell("E2").Value = "Preço";
            worksheet.Cell("F2").Value = "Total";
            worksheet.Cell("G2").Value = "Cliente";
            worksheet.Cell("H2").Value = "Data da Venda";
            worksheet.Cell("I2").Value = "Hora da Venda";
            foreach (Venda venda in VendaViewModel.Vendas)
            {
                worksheet.Cell("A" + x).Value = venda.IdVenda;
                worksheet.Cell("B" + x).Value = venda.Modelo.Nome;
                worksheet.Cell("C" + x).Value = venda.Tamanho;
                worksheet.Cell("D" + x).Value = venda.QtdItens;
                worksheet.Cell("E" + x).Value = String.Format("R$ {0}", venda.Preco);
                worksheet.Cell("F" + x).Value = String.Format("R$ {0}", venda.Total);
                worksheet.Cell("G" + x).Value = venda.Cliente;
                worksheet.Cell("H" + x).Value = String.Format("{0:dd/MM/yyyy}", venda.DataVenda);
                worksheet.Cell("I" + x).Value = String.Format("{0:HH:mm:ss}", venda.DataVenda);
                x++;
            }
            var rngTable = worksheet.Range("A1:I" + (x - 1));
            rngTable.Cell(1, 1).Style.Font.Bold = true;
            rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.Gray;
            rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngTable.Row(1).Merge();

            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            var rngHeaders = rngTable.Range("A2:I2");
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.LightGray;

            worksheet.Columns(1, 9).AdjustToContents();

            // workbook.SaveAs("BasicTable.xlsx");
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Arquivos do Excel|*.xlsx",
                Title = "Salvar arquivo do Excel",
                InitialDirectory = pathRelatorios,
                FileName = "Relatorio_Vendas_" + String.Format("{0:ddMMyyyy}", DateTime.Now)
            };

            saveFileDialog.ShowDialog();

            if (!String.IsNullOrWhiteSpace(saveFileDialog.FileName))
                workbook.SaveAs(saveFileDialog.FileName);
        }
        //Relatorio de clientes
        public void GerarXmlClientes()
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Relatorio de Clientes");
            int x = 3;
            worksheet.Cell("A1").Value = "Relatorio de Clientes - Gerado em " + String.Format("{0:dd/MM/yyyy} as {0:HH:mm:ss}", DateTime.Now);
            worksheet.Cell("A2").Value = "#";
            worksheet.Cell("B2").Value = "Nome";
            worksheet.Cell("C2").Value = "Cpf";
            worksheet.Cell("D2").Value = "Data de Nascimento";
            worksheet.Cell("E2").Value = "Razão Social";
            worksheet.Cell("F2").Value = "Cnpj";
            worksheet.Cell("G2").Value = "Rua";
            worksheet.Cell("H2").Value = "Numero";
            worksheet.Cell("I2").Value = "Estado";
            worksheet.Cell("J2").Value = "Cidade";
            worksheet.Cell("K2").Value = "Bairro";
            foreach (Pessoa pessoa in PessoaViewModel.Clientes)
            {
                if (pessoa.GetType() == typeof(PessoaFisica))
                {
                    var pessoaf = (PessoaFisica)pessoa;
                    worksheet.Cell("A" + x).Value = pessoaf.IdPessoa;
                    worksheet.Cell("B" + x).Value = pessoaf.Nome;
                    worksheet.Cell("C" + x).Value = pessoaf.Cpf;
                    worksheet.Cell("D" + x).Value = String.Format("{0:dd/MM/yyyy}", pessoaf.DataNascimento);
                }
                else
                {
                    var pessoaj = (PessoaJuridica)pessoa;
                    worksheet.Cell("A" + x).Value = pessoaj.IdPessoa;
                    worksheet.Cell("B" + x).Value = pessoaj.Nome;
                    worksheet.Cell("E" + x).Value = pessoaj.RazaoSocial;
                    worksheet.Cell("F" + x).Value = pessoaj.Cnpj;
                    worksheet.Cell("G" + x).Value = pessoaj.Endereco.Rua;
                    worksheet.Cell("H" + x).Value = pessoaj.Endereco.Numero;
                    worksheet.Cell("I" + x).Value = pessoaj.Endereco.Estado;
                    worksheet.Cell("J" + x).Value = pessoaj.Endereco.Cidade;
                    worksheet.Cell("K" + x).Value = pessoaj.Endereco.Bairro;
                }
                x++;
            }
            var rngTable = worksheet.Range("A1:K" + (x - 1));
            rngTable.Cell(1, 1).Style.Font.Bold = true;
            rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.Gray;
            rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngTable.Row(1).Merge();

            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            var rngHeaders = rngTable.Range("A2:K2");
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.LightGray;

            worksheet.Columns(1, 11).AdjustToContents();

            // workbook.SaveAs("BasicTable.xlsx");
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Arquivos do Excel|*.xlsx",
                Title = "Salvar arquivo do Excel",
                InitialDirectory = pathRelatorios,
                FileName = "Relatorio_Clientes_" + String.Format("{0:ddMMyyyy}", DateTime.Now)
            };

            saveFileDialog.ShowDialog();

            if (!String.IsNullOrWhiteSpace(saveFileDialog.FileName))
                workbook.SaveAs(saveFileDialog.FileName);
        }
    }
}
