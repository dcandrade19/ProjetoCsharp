namespace SistemaSapatos.Base.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pessoas",
                c => new
                    {
                        IdPessoa = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Cpf = c.String(),
                        DataNascimento = c.DateTime(),
                        RazaoSocial = c.String(),
                        Cnpj = c.String(),
                        Endereco_IdEndereco = c.Int(),
                        Endereco_Rua = c.String(),
                        Endereco_Numero = c.Int(),
                        Endereco_Bairro = c.String(),
                        Endereco_Cidade = c.String(),
                        Endereco_Estado = c.String(),
                        Endereco_Complemento = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.IdPessoa);
            
            CreateTable(
                "dbo.Vendas",
                c => new
                    {
                        IdVenda = c.Int(nullable: false, identity: true),
                        IdModelo = c.Int(nullable: false),
                        QtdItens = c.Int(nullable: false),
                        Tamanho = c.Int(nullable: false),
                        Preco = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IdPessoa = c.Int(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataVenda = c.DateTime(),
                        NomeModeloPreCriacao = c.String(),
                    })
                .PrimaryKey(t => t.IdVenda)
                .ForeignKey("dbo.Pessoas", t => t.IdPessoa, cascadeDelete: true)
                .ForeignKey("dbo.Modelos", t => t.IdModelo, cascadeDelete: true)
                .Index(t => t.IdModelo)
                .Index(t => t.IdPessoa);
            
            CreateTable(
                "dbo.Modelos",
                c => new
                    {
                        IdModelo = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Cardarco = c.Boolean(nullable: false),
                        Material = c.String(),
                        Cor = c.String(),
                        Preco = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalEstoque = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdModelo);
            
            CreateTable(
                "dbo.Estoques",
                c => new
                    {
                        IdEstoque = c.Int(nullable: false, identity: true),
                        Tamanho = c.Int(nullable: false),
                        Quantidade = c.Int(nullable: false),
                        IdModelo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdEstoque)
                .ForeignKey("dbo.Modelos", t => t.IdModelo, cascadeDelete: true)
                .Index(t => t.IdModelo);
            
            CreateTable(
                "dbo.Imagens",
                c => new
                    {
                        IdImagem = c.Int(nullable: false, identity: true),
                        Imagem = c.Binary(),
                        Modelo_IdModelo = c.Int(),
                    })
                .PrimaryKey(t => t.IdImagem)
                .ForeignKey("dbo.Modelos", t => t.Modelo_IdModelo)
                .Index(t => t.Modelo_IdModelo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vendas", "IdModelo", "dbo.Modelos");
            DropForeignKey("dbo.Imagens", "Modelo_IdModelo", "dbo.Modelos");
            DropForeignKey("dbo.Estoques", "IdModelo", "dbo.Modelos");
            DropForeignKey("dbo.Vendas", "IdPessoa", "dbo.Pessoas");
            DropIndex("dbo.Imagens", new[] { "Modelo_IdModelo" });
            DropIndex("dbo.Estoques", new[] { "IdModelo" });
            DropIndex("dbo.Vendas", new[] { "IdPessoa" });
            DropIndex("dbo.Vendas", new[] { "IdModelo" });
            DropTable("dbo.Imagens");
            DropTable("dbo.Estoques");
            DropTable("dbo.Modelos");
            DropTable("dbo.Vendas");
            DropTable("dbo.Pessoas");
        }
    }
}
