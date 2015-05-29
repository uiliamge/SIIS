namespace SIIS.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RetireiTipoPermissaoDoUserEColoqueiEmPaciente : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "TipoPermissao");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "TipoPermissao", c => c.Int(nullable: false));
        }
    }
}
