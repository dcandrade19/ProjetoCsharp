using SistemaSapatosBase.Model;
using System.Data.Entity;

namespace SistemaSapatosBase.DataBase
{
    /// <summary>
    /// Classe de contexto extende a classe DbContext do entityframework
    /// </summary>
    class Contexto : DbContext
    {
        public Contexto()
            : base("name=sqlServer")
        {

        }
        // DbSet é uma coleção do Entity Framework que recebe as entidades que vão ser salvas em banco
        public DbSet<Pessoa> Clientes { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Modelo> Modelos { get; set; }
        public DbSet<Estoque> Estoques { get; set; }

    }
}
