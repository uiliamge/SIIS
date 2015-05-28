namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdEmPacienteEResponsavel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Paciente", "Id", "dbo.Usuario");
            DropForeignKey("dbo.Responsavel", "Id", "dbo.Usuario");
            DropForeignKey("dbo.Paciente", "Id", "dbo.Pessoa");
            DropForeignKey("dbo.Responsavel", "Id", "dbo.Pessoa");
            DropIndex("dbo.PermissaoResponsavelPaciente", new[] { "Paciente_Id" });
            DropPrimaryKey("dbo.Pessoa");
            AddColumn("dbo.Responsavel", "UserId", c => c.String());
            AddColumn("dbo.Paciente", "UserId", c => c.String());
            AddColumn("dbo.Usuario", "Paciente_Id", c => c.Int());
            AddColumn("dbo.Usuario", "Responsavel_Id", c => c.Int());
            AlterColumn("dbo.Pessoa", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.PermissaoResponsavelPaciente", "Paciente_Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Pessoa", "Id");
            CreateIndex("dbo.PermissaoResponsavelPaciente", "Paciente_Id");
            CreateIndex("dbo.Usuario", "Paciente_Id");
            CreateIndex("dbo.Usuario", "Responsavel_Id");
            AddForeignKey("dbo.Usuario", "Paciente_Id", "dbo.Paciente", "Id");
            AddForeignKey("dbo.Usuario", "Responsavel_Id", "dbo.Responsavel", "Id");
            AddForeignKey("dbo.Paciente", "Id", "dbo.Pessoa", "Id");
            AddForeignKey("dbo.Responsavel", "Id", "dbo.Pessoa", "Id");
            DropColumn("dbo.PermissaoResponsavelPaciente", "PacienteId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PermissaoResponsavelPaciente", "PacienteId", c => c.String(nullable: false));
            DropForeignKey("dbo.Responsavel", "Id", "dbo.Pessoa");
            DropForeignKey("dbo.Paciente", "Id", "dbo.Pessoa");
            DropForeignKey("dbo.Usuario", "Responsavel_Id", "dbo.Responsavel");
            DropForeignKey("dbo.Usuario", "Paciente_Id", "dbo.Paciente");
            DropIndex("dbo.Usuario", new[] { "Responsavel_Id" });
            DropIndex("dbo.Usuario", new[] { "Paciente_Id" });
            DropIndex("dbo.PermissaoResponsavelPaciente", new[] { "Paciente_Id" });
            DropPrimaryKey("dbo.Pessoa");
            AlterColumn("dbo.PermissaoResponsavelPaciente", "Paciente_Id", c => c.Int());
            AlterColumn("dbo.Pessoa", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.Usuario", "Responsavel_Id");
            DropColumn("dbo.Usuario", "Paciente_Id");
            DropColumn("dbo.Paciente", "UserId");
            DropColumn("dbo.Responsavel", "UserId");
            AddPrimaryKey("dbo.Pessoa", "Id");
            CreateIndex("dbo.PermissaoResponsavelPaciente", "Paciente_Id");
            AddForeignKey("dbo.Responsavel", "Id", "dbo.Pessoa", "Id");
            AddForeignKey("dbo.Paciente", "Id", "dbo.Pessoa", "Id");
            AddForeignKey("dbo.Responsavel", "Id", "dbo.Usuario", "Id");
            AddForeignKey("dbo.Paciente", "Id", "dbo.Usuario", "Id");
        }
    }
}
