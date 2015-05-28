using System.Data.Entity.Migrations;

namespace SIIS.Migrations
{
    public partial class PermissaoResponsavelPaciente : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PermissaoResponsavelPaciente", "Responsavel_Id", "dbo.Responsavel");
            DropIndex("dbo.PermissaoResponsavelPaciente", new[] { "Paciente_Id" });
            DropIndex("dbo.PermissaoResponsavelPaciente", new[] { "Responsavel_Id" });
            AddColumn("dbo.PermissaoResponsavelPaciente", "PacienteId", c => c.String(nullable: false));
            AddColumn("dbo.PermissaoResponsavelPaciente", "NumeroConselho", c => c.Int(nullable: false));
            AddColumn("dbo.PermissaoResponsavelPaciente", "SiglaConselhoRegional", c => c.Int(nullable: false));
            AddColumn("dbo.PermissaoResponsavelPaciente", "UfConselhoRegional", c => c.Int(nullable: false));
            AlterColumn("dbo.PermissaoResponsavelPaciente", "Paciente_Id", c => c.Int());
            CreateIndex("dbo.PermissaoResponsavelPaciente", "Paciente_Id");
            DropColumn("dbo.PermissaoResponsavelPaciente", "Responsavel_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PermissaoResponsavelPaciente", "Responsavel_Id", c => c.Int(nullable: false));
            DropIndex("dbo.PermissaoResponsavelPaciente", new[] { "Paciente_Id" });
            AlterColumn("dbo.PermissaoResponsavelPaciente", "Paciente_Id", c => c.Int(nullable: false));
            DropColumn("dbo.PermissaoResponsavelPaciente", "UfConselhoRegional");
            DropColumn("dbo.PermissaoResponsavelPaciente", "SiglaConselhoRegional");
            DropColumn("dbo.PermissaoResponsavelPaciente", "NumeroConselho");
            DropColumn("dbo.PermissaoResponsavelPaciente", "PacienteId");
            CreateIndex("dbo.PermissaoResponsavelPaciente", "Responsavel_Id");
            CreateIndex("dbo.PermissaoResponsavelPaciente", "Paciente_Id");
            AddForeignKey("dbo.PermissaoResponsavelPaciente", "Responsavel_Id", "dbo.Responsavel", "Id");
        }
    }
}
