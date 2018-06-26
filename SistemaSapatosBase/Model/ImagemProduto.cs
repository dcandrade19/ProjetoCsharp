using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaSapatosBase.Model
{
    [Table("Imagens")]
    public class ImagemProduto
    {
        [Key]
        public int IdImagem { get; set; }
        public byte[] Imagem { get; set; }
    }
}
