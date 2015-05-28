namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReinclusaoPaciente : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Paciente",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Nome = c.String(nullable: false),
                        CpfCnpj = c.String(nullable: false, maxLength: 14),
                        Email = c.String(nullable: false),
                        Cep = c.String(nullable: false),
                        Endereco = c.String(),
                        Uf = c.Int(nullable: false),
                        Cidade = c.String(nullable: false),
                        Telefone = c.String(),
                        Ip = c.String(nullable: false),
                        DataHora = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Composicao", "Paciente_Id", c => c.Int());
            AddColumn("dbo.PermissaoResponsavelPaciente", "Paciente_Id", c => c.Int());
            CreateIndex("dbo.Composicao", "Paciente_Id");
            CreateIndex("dbo.PermissaoResponsavelPaciente", "Paciente_Id");
            AddForeignKey("dbo.Composicao", "Paciente_Id", "dbo.Paciente", "Id");
            AddForeignKey("dbo.PermissaoResponsavelPaciente", "Paciente_Id", "dbo.Paciente", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PermissaoResponsavelPaciente", "Paciente_Id", "dbo.Paciente");
            DropForeignKey("dbo.Composicao", "Paciente_Id", "dbo.Paciente");
            DropIndex("dbo.PermissaoResponsavelPaciente", new[] { "Paciente_Id" });
            DropIndex("dbo.Composicao", new[] { "Paciente_Id" });
            DropColumn("dbo.PermissaoResponsavelPaciente", "Paciente_Id");
            DropColumn("dbo.Composicao", "Paciente_Id");
            DropTable("dbo.Paciente");
        }
    }
}
