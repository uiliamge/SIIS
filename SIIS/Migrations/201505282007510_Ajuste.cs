namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ajuste : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Composicao", "Paciente_Id", "dbo.Paciente");
            DropForeignKey("dbo.PermissaoResponsavelPaciente", "Paciente_Id", "dbo.Paciente");
            DropForeignKey("dbo.LogAcessoResponsavel", "Paciente_Id", "dbo.Paciente");
            DropForeignKey("dbo.Usuario", "Paciente_Id", "dbo.Paciente");
            DropIndex("dbo.Composicao", new[] { "Paciente_Id" });
            DropIndex("dbo.LogAcessoResponsavel", new[] { "Paciente_Id" });
            DropIndex("dbo.PermissaoResponsavelPaciente", new[] { "Paciente_Id" });
            DropIndex("dbo.Usuario", new[] { "Paciente_Id" });
            DropColumn("dbo.Composicao", "Paciente_Id");
            DropColumn("dbo.LogAcessoResponsavel", "Paciente_Id");
            DropColumn("dbo.PermissaoResponsavelPaciente", "Paciente_Id");
            DropColumn("dbo.Usuario", "Paciente_Id");
            DropTable("dbo.Paciente");
        }
        
        public override void Down()
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
                        fone = c.String(),
                        Ip = c.String(nullable: false),
                        DataHora = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Usuario", "Paciente_Id", c => c.Int());
            AddColumn("dbo.PermissaoResponsavelPaciente", "Paciente_Id", c => c.Int(nullable: false));
            AddColumn("dbo.LogAcessoResponsavel", "Paciente_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Composicao", "Paciente_Id", c => c.Int());
            CreateIndex("dbo.Usuario", "Paciente_Id");
            CreateIndex("dbo.PermissaoResponsavelPaciente", "Paciente_Id");
            CreateIndex("dbo.LogAcessoResponsavel", "Paciente_Id");
            CreateIndex("dbo.Composicao", "Paciente_Id");
            AddForeignKey("dbo.Usuario", "Paciente_Id", "dbo.Paciente", "Id");
            AddForeignKey("dbo.LogAcessoResponsavel", "Paciente_Id", "dbo.Paciente", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PermissaoResponsavelPaciente", "Paciente_Id", "dbo.Paciente", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Composicao", "Paciente_Id", "dbo.Paciente", "Id");
        }
    }
}
