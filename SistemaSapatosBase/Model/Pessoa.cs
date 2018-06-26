using SistemaSapatos.Base.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSapatosBase.Model
{
    [Table("Pessoas")]
    public abstract class Pessoa : EntidadeBase
    {
        
        private int _idPessoa;
        private String _nome;
        private ObservableCollection<Venda> _compras = new ObservableCollection<Venda>();

        // Define o 'idPessoa' como chave primaria para tabela "pessoa"
        [Key]
        public int IdPessoa
        {
            get { return _idPessoa; }
            set { _idPessoa = value;Notificacao(); }
        }

        public String Nome
        {
            get { return _nome; }
            set { _nome = value;Notificacao(); }
        }

        public ObservableCollection<Venda> Compras
        {
            get { return _compras; }
            set { _compras = value;Notificacao(); }
        }

        public override string ToString()
        {
            return Nome;
        }

    }
}
