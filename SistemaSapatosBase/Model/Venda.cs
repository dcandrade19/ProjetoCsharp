using SistemaSapatos.Base.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSapatosBase.Model
{
    [Table("Vendas")]
    public class Venda : EntidadeBase
    {
        
        // As classes que forem se tornar tabelas no banco precisam de um atributo "id" neste caso "IdVenda"

        private int _idVenda;
        private int _idModelo;
        private Modelo _modelo;
        private int _qtdItens;
        private int _tamanho;
        private Decimal _preco;
        private int _idPessoa;
        private Pessoa _cliente;
        private Decimal _total;
        private DateTime? _dataVenda =  DateTime.Now;
        [NotMapped]
        private string _nomeModeloPreCriacao;
        
        [Key]
        public int IdVenda
        {
            get { return _idVenda; }
            set { _idVenda = value; Notificacao(); }
        }
        [ForeignKey("Modelo")]
        public int IdModelo
        {
            get { return _idModelo; }
            set { _idModelo = value;Notificacao(); }
        }
        public Modelo Modelo
        {
            get { return _modelo; }
            set { _modelo = value; Notificacao(); }
        }

        public int QtdItens
        {
            get { return _qtdItens; }
            set { _qtdItens = value; Notificacao(); setTotal(); }
        }

        public int Tamanho
        {
            get { return _tamanho; }
            set { _tamanho = value; Notificacao(); }
        }

        public Decimal Preco
        {
            get { return _preco; }
            set { _preco = value; Notificacao(); setTotal(); }
        }
        [ForeignKey("Cliente")]
        public int IdPessoa
        {
            get { return _idPessoa; }
            set { _idPessoa = value;Notificacao(); }
        }
        public Pessoa Cliente
        {
            get { return _cliente; }
            set { _cliente = value; Notificacao(); }
        }

        public Decimal Total
        {
            get { return _total; }
            set { _total = value; Notificacao(); }
        }

        public DateTime? DataVenda
        {
            get { return _dataVenda; }
            set { _dataVenda = value;Notificacao(); }
        }

        public string NomeModeloPreCriacao
        {
            get { return _nomeModeloPreCriacao; }
            set { _nomeModeloPreCriacao = value;Notificacao(); }
        }

        public void setTotal()
        {
            Total = _qtdItens * _preco;
        }
    }
}
