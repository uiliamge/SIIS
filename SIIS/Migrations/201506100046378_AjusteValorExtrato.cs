namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteValorExtrato : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Extrato", "ValorCobrado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Extrato", "ValorCobrado");
        }
    }
}
