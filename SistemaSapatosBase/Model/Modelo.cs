using SistemaSapatos.Base.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSapatosBase.Model
{

    [Table("Modelos")]
    public class  Modelo : EntidadeBase
    {
        private int _idModelo;
        private string _nome;
        private bool _cardarco;
        private string _material;
        private string _cor;
        private decimal _preco;
        private ICollection<ImagemProduto> _imagens;
        private ObservableCollection<Estoque> _estoques = new ObservableCollection<Estoque>();
        private ICollection<Venda> _vendas;
        private int _totalEstoque;

        [Key]
        public int IdModelo
        {
            get { return _idModelo; }
            set { _idModelo = value;Notificacao(); }
        }
       
        public string Nome
        {
            get { return _nome; }
            set { _nome = value;Notificacao(); }
        }

        public bool Cardarco
        {
            get { return _cardarco; }
            set { _cardarco = value; Notificacao(); }
        }

        public string Material
        {
            get { return _material; }
            set { _material = value; Notificacao(); }
        }

        public string Cor
        {
            get { return _cor; }
            set { _cor = value; Notificacao(); }
        }

        public decimal Preco
        {
            get { return _preco; }
            set { _preco = value; Notificacao(); }
        }

        public ICollection<ImagemProduto> Imagens
        {
            get { return _imagens; }
            set { _imagens = value; Notificacao(); }
        }

        public ObservableCollection<Estoque> Estoques
        {
            get { return _estoques; }
            set { _estoques = value; Notificacao();}
        }

        public ICollection<Venda> Vendas
        {
            get { return _vendas; }
            set { _vendas = value;Notificacao(); }
        }
        /// <summary>
        /// Atualiza o total de itens em estoque
        /// </summary>
        public int TotalEstoque
        {
            get { return _totalEstoque = Total();}
            set { _totalEstoque = Total();Notificacao(); }
        }
        /// <summary>
        /// Realiza os calculos necessarios para definir a quantidade total
        /// de estoque
        /// </summary>
        /// <returns></returns>
        public int Total()
        {
            int total = 0;
            if (Estoques != null)
            {
                foreach (Estoque est in Estoques)
                {
                    total += est.Quantidade;
                }
            }
            return total;
        }

        public override string ToString()
        {

            return Nome;
        }
    }
}
