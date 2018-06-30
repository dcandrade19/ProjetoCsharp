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
        // ViewModel de Clientes
        private PessoaViewModel PessoaViewModel { get; set; }
        // ViewModel de Vendas
        private VendaViewModel VendaViewModel { get; set; }
        #endregion
        // Caminho para gerar o diretorio de relatorios
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
        /// <summary>
        /// Chama o metodo responsavel para salvar o modelo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            ModeloViewModel.SalvarComando();
            LimparModelo();
        }
        /// <summary>
        /// Chama o metodo responsavel para limpar o modelo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLimparModelo_Click(object sender, RoutedEventArgs e)
        {
            LimparModelo();
        }
        /// <summary>
        /// Chama o metodo responsavel por adicionar um item ao estoque
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddEstoque_Click(object sender, RoutedEventArgs e)
        {
            AdicionarEstoque();
        }
        /// <summary>
        /// Chama o metodo responsavel para deletar um modelo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeletar_Click(object sender, RoutedEventArgs e)
        {
            ModeloViewModel.DeletarComando();
        }

        
        /// <summary>
        /// Inicializa os combobox da tela de modelos com seus valores padrão
        /// </summary>
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

        /// <summary>
        /// Adiciona ou atualiza um item de estoque na tela de modelos
        /// </summary>
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

        /// <summary>
        /// Limpa a tela de modelos setando um modelo vazio como atual
        /// </summary>
        public void LimparModelo()
        {
            dataModelos.SelectedIndex = -1;
            dataModelos.SelectedItem = new Modelo();
        }
        #endregion

        #region Tela Clientes // Metodos da tela de clientes
        /// <summary>
        /// Chama o metodo responsavel para adicionar uma venda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddVenda_Click(object sender, RoutedEventArgs e)
        {
            //modeloViewModel.PreVendaComando();
            AdicionarVenda();
            cbxModelo.SelectedIndex = -1;
        }
        /// <summary>
        /// Chama o metodo responsavel para calcular a quantidade de estoque disponivel
        /// para o tamanho selecionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxTamanho_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModeloViewModel.CalcularQuantidadeDisponivel();
        }
        /// <summary>
        /// Chama o metodo responsavel para remover um item de venda e reseta
        /// o combobox modelo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRemover_Click(object sender, RoutedEventArgs e)
        {
            //modeloViewModel.RemoverPreVendaComando();
            RemoverVenda();
            cbxModelo.SelectedIndex = -1;
        }

        /// <summary>
        /// Cria um objeto Venda e atualiza os estoques do modelo escolhido
        /// </summary>
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
        /// <summary>
        /// Remove uma venda da lista de compras do cliente e atualiza o estoque do modelo referente
        /// </summary>
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
        /// <summary>
        /// Remove todas as vendas não finalizadas da lista de compras do cliente
        /// </summary>
        public void RemoverVendasLotes()
        {
            var compras = PessoaViewModel.ClienteSelecionado.Compras.ToList();
            foreach (Venda venda in compras)
            {
                PessoaViewModel.CompraSelecionada = venda;
                RemoverVenda();
            }
        }
        /// <summary>
        /// Configura a exibição para informações de pessoa fisica
        /// e chama o metodo DefinirTipo da view de clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RbnPessoaFisica_Checked(object sender, RoutedEventArgs e)
        {
            PessoaViewModel.CpfCnpjBusca = "";
            rbnPessoaJuridica.IsChecked = false;
            txbCpfCnpj.Text = "CPF:";
            PessoaViewModel.DefinirTipo();
        }
        /// <summary>
        /// Configura a exibição para informações de pessoa juridica
        /// e chama o metodo DefinirTipo da view de clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RbnPessoaJuridica_Checked(object sender, RoutedEventArgs e)
        {
            PessoaViewModel.CpfCnpjBusca = "";
            rbnPessoaFisica.IsChecked = false;
            txbCpfCnpj.Text = "CNPJ:";
            PessoaViewModel.DefinirTipo();
        }
        /// <summary>
        /// Chama os metodos responsaveis por salvar o cliente e
        /// editar os modelos de cada venda deste cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Chama os metodos responsaveis para deletar um cliente e todas
        /// as vendas associadas a ele que ainda não foram finalizadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeletarCliente_Click(object sender, RoutedEventArgs e)
        {
            RemoverVendasLotes();
            PessoaViewModel.DeletarComando();
            LimparCliente();
        }
        /// <summary>
        /// Chama o metodo responsavel para limpar a tela de cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLimparCliente_Click(object sender, RoutedEventArgs e)
        {
            LimparCliente();
        }
        /// <summary>
        /// Chama o metodo de limpar a tela do cliente e em seguida
        /// o metodo de busca de cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBuscarCliente_Click(object sender, RoutedEventArgs e)
        {
            LimparCliente();
            PessoaViewModel.BuscarClienteComando();
        }
        /// <summary>
        /// Limpa a tela de Clientes setando um cliente vazio como atual
        /// e remove todas as vendas não finalizadas referentes
        /// </summary>
        public void LimparCliente()
        {
            RemoverVendasLotes();
            PessoaViewModel.ClienteSelecionado = new PessoaFisica();
        }

        #endregion

        /// <summary>
        /// Chama o metodo responsavel para gerar o relatorio de vendas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnGerarXmlVendas_Click(object sender, RoutedEventArgs e)
        {
            GerarXmlVendas();
        }
        /// <summary>
        /// Chama o metodo responsavel para gerar o relatorio de clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnGerarXmlClientes_Click(object sender, RoutedEventArgs e)
        {
            GerarXmlClientes();
        }
        /// <summary>
        /// Gera um arquivo xml com todos os dados das vendas realizadas
        /// </summary>
        public void GerarXmlVendas()
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Relatorio de Vendas");
            int x = 3;
            worksheet.Cell("A1").Value = "Relatorio de Vendas - Gerado em " + String.Format("{0:dd/MM/yyyy} as {0:HH:mm:ss}", DateTime.Now);
            worksheet.Cell("A2").Value = "#";
            worksheet.Cell("B2").Value = "Modelo";
            worksheet.Cell("C2").Value = "Tamanho";
            worksheet.Cell("D2").Value = "Cliente";
            worksheet.Cell("E2").Value = "Data da Venda";
            worksheet.Cell("F2").Value = "Hora da Venda";
            worksheet.Cell("G2").Value = "Preço(u)";
            worksheet.Cell("H2").Value = "Quantidade";
            worksheet.Cell("I2").Value = "Total";
            foreach (Venda venda in VendaViewModel.Vendas)
            {
                worksheet.Cell("A" + x).Value = venda.IdVenda;
                worksheet.Cell("B" + x).Value = venda.Modelo.Nome;
                worksheet.Cell("C" + x).Value = venda.Tamanho;
                worksheet.Cell("D" + x).Value = venda.Cliente;
                worksheet.Cell("E" + x).Value = String.Format("{0:dd/MM/yyyy}", venda.DataVenda);
                worksheet.Cell("F" + x).Value = String.Format("{0:HH:mm:ss}", venda.DataVenda);
                worksheet.Cell("G" + x).Value = venda.Preco;
                worksheet.Cell("H" + x).Value = venda.QtdItens;
                worksheet.Cell("I" + x).Value = venda.Total;
                x++;
            }
            worksheet.Cell("G" + x).Value = "Totais";
            worksheet.Cell("H" + x).FormulaA1 = "SUM(H3:H" + (x - 1) + ")";
            worksheet.Cell("I" + x).FormulaA1 = "SUM(I3:I" + (x - 1) + ")";
            //worksheet.Cell("D" + x).Value = null;

            var rngTable = worksheet.Range("A1:I" + (x));
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

            var rngFooter = rngTable.Range(string.Format("A{0}:I{0}", x));
            rngFooter.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            rngFooter.Style.Font.Bold = true;
            rngFooter.Style.Fill.BackgroundColor = XLColor.LightGray;

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
        /// <summary>
        /// Gera um arquivo xml com todos os clientes cadastrados no banco
        /// </summary>
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
