namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlanoDeSaudeEValorNoExtrato : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Extrato", "PlanoSaude", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Extrato", "PlanoSaude");
        }
    }
}
