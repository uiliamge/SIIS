namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IndiceEmComposicaoESecao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Composicao", "Indice", c => c.Int(nullable: false));
            AddColumn("dbo.Extrato", "Cidade", c => c.String(nullable: false));
            AddColumn("dbo.Extrato", "Uf", c => c.Int(nullable: false));
            AddColumn("dbo.Secao", "Indice", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Secao", "Indice");
            DropColumn("dbo.Extrato", "Uf");
            DropColumn("dbo.Extrato", "Cidade");
            DropColumn("dbo.Composicao", "Indice");
        }
    }
}
