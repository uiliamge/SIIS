namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PermissaoExibicaoExtrato : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Extrato", "ExibicaoPermitida", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Extrato", "ExibicaoPermitida");
        }
    }
}
