namespace SIIS.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NovosCamposNoApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TipoUsuario", c => c.String());
            AddColumn("dbo.AspNetUsers", "NumeroConselho", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "SiglaConselhoRegional", c => c.String());
            AddColumn("dbo.AspNetUsers", "UfConselhoRegional", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Cpf", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Cpf");
            DropColumn("dbo.AspNetUsers", "UfConselhoRegional");
            DropColumn("dbo.AspNetUsers", "SiglaConselhoRegional");
            DropColumn("dbo.AspNetUsers", "NumeroConselho");
            DropColumn("dbo.AspNetUsers", "TipoUsuario");
        }
    }
}
