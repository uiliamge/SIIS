namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class extratoEComposicao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Extrato", "CpfPaciente", c => c.String(nullable: false, maxLength: 18));
            DropColumn("dbo.Secao", "IndiceComposicao");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Secao", "IndiceComposicao", c => c.Int(nullable: false));
            DropColumn("dbo.Extrato", "CpfPaciente");
        }
    }
}
