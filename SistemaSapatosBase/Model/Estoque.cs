using SistemaSapatos.Base.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaSapatosBase.Model
{
    [Table("Estoques")]
    public class Estoque : EntidadeBase
    {
        
        private int _idEstoque;
        private int _tamanho;
        private int _quantidade;
        private int _idModelo;
        private Modelo _modelo;

        [Key]
        public int IdEstoque
        {
            get { return _idEstoque; }
            set { _idEstoque = value; Notificacao(); }
        }
        public int Tamanho
        {
            get { return _tamanho; }
            set { _tamanho = value;Notificacao(); }
        }
        public int Quantidade
        {
            get { return _quantidade; }
            set { _quantidade = value;Notificacao();}
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
            set { _modelo = value;Notificacao(); }
        }
        public override string ToString()
        {
            return Tamanho.ToString();
        }
    }
}
