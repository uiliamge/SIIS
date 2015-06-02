namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteResponsavel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Responsavel", "SiglaConselhoRegional", c => c.Int(nullable: false));
            DropColumn("dbo.Responsavel", "ConselhoRegional");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Responsavel", "ConselhoRegional", c => c.Int(nullable: false));
            DropColumn("dbo.Responsavel", "SiglaConselhoRegional");
        }
    }
}
