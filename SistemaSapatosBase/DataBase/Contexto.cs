using SistemaSapatosBase.Model;
using System.Data.Entity;

namespace SistemaSapatosBase.DataBase
{
    // Essa é a classe de contexto ela extende a classe "DbContext" que é uma classe do Entity Framework
    class Contexto : DbContext
    {
        public Contexto()
            : base("name=sqlServer")
        {

        }
        // DbSet é uma coleção do Entity Framework que recebe a Entitade que vc vai salvar no banco
        public DbSet<Pessoa> Clientes { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Modelo> Modelos { get; set; }
        public DbSet<Estoque> Estoques { get; set; }

    }
}
