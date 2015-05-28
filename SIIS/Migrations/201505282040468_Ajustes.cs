namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ajustes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Usuario", "Responsavel_Id", "dbo.Responsavel");
            DropForeignKey("dbo.PermissaoResponsavelPaciente", "Paciente_Id", "dbo.Paciente");
            DropIndex("dbo.PermissaoResponsavelPaciente", new[] { "Paciente_Id" });
            DropIndex("dbo.Usuario", new[] { "Responsavel_Id" });
            AddColumn("dbo.LogAcessoResponsavel", "Paciente_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.PermissaoResponsavelPaciente", "Paciente_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.LogAcessoResponsavel", "Paciente_Id");
            CreateIndex("dbo.PermissaoResponsavelPaciente", "Paciente_Id");
            AddForeignKey("dbo.LogAcessoResponsavel", "Paciente_Id", "dbo.Paciente", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PermissaoResponsavelPaciente", "Paciente_Id", "dbo.Paciente", "Id", cascadeDelete: true);
            DropTable("dbo.Usuario");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.Int(nullable: false),
                        Password = c.String(nullable: false),
                        TipoUsuario = c.Int(nullable: false),
                        Responsavel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.PermissaoResponsavelPaciente", "Paciente_Id", "dbo.Paciente");
            DropForeignKey("dbo.LogAcessoResponsavel", "Paciente_Id", "dbo.Paciente");
            DropIndex("dbo.PermissaoResponsavelPaciente", new[] { "Paciente_Id" });
            DropIndex("dbo.LogAcessoResponsavel", new[] { "Paciente_Id" });
            AlterColumn("dbo.PermissaoResponsavelPaciente", "Paciente_Id", c => c.Int());
            DropColumn("dbo.LogAcessoResponsavel", "Paciente_Id");
            CreateIndex("dbo.Usuario", "Responsavel_Id");
            CreateIndex("dbo.PermissaoResponsavelPaciente", "Paciente_Id");
            AddForeignKey("dbo.PermissaoResponsavelPaciente", "Paciente_Id", "dbo.Paciente", "Id");
            AddForeignKey("dbo.Usuario", "Responsavel_Id", "dbo.Responsavel", "Id");
        }
    }
}
