namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjustesEmPessoa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pessoa", "fone", c => c.String());
            AlterColumn("dbo.Pessoa", "Endereco", c => c.String());
            AlterColumn("dbo.Pessoa", "Uf", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pessoa", "Uf", c => c.String(nullable: false));
            AlterColumn("dbo.Pessoa", "Endereco", c => c.String(nullable: false));
            DropColumn("dbo.Pessoa", "fone");
        }
    }
}
