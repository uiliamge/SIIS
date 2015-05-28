using System.Data.Entity.Migrations;

namespace SIIS.Migrations.Identity
{
    public partial class DadosParaPacienteNoApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TipoPermissao", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "TipoUsuario", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "TipoUsuario", c => c.String());
            DropColumn("dbo.AspNetUsers", "TipoPermissao");
        }
    }
}
