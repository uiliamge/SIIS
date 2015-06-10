namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteTamanhoTextoComposicaoSecao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Secao", "Descricao", c => c.String(maxLength: 2000));
            AlterColumn("dbo.Composicao", "Descricao", c => c.String(maxLength: 2000));
            DropColumn("dbo.Secao", "Nome");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Secao", "Nome", c => c.String(maxLength: 100));
            AlterColumn("dbo.Composicao", "Descricao", c => c.String(maxLength: 100));
            DropColumn("dbo.Secao", "Descricao");
        }
    }
}
