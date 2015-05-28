namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExclusaoPessoa : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Paciente", "Id", "dbo.Pessoa");
            DropForeignKey("dbo.Responsavel", "Id", "dbo.Pessoa");
            DropForeignKey("dbo.LogAcessoResponsavel", "Paciente_Id", "dbo.Paciente");
            DropForeignKey("dbo.LogAcessoResponsavel", "Responsavel_Id", "dbo.Responsavel");
            DropForeignKey("dbo.PermissaoResponsavelPaciente", "Paciente_Id", "dbo.Paciente");
            DropForeignKey("dbo.Extrato", "Responsavel_Id", "dbo.Responsavel");
            DropForeignKey("dbo.Usuario", "Responsavel_Id", "dbo.Responsavel");
            DropForeignKey("dbo.Composicao", "Paciente_Id", "dbo.Paciente");
            DropForeignKey("dbo.Usuario", "Paciente_Id", "dbo.Paciente");
            DropIndex("dbo.Paciente", new[] { "Id" });
            DropIndex("dbo.Responsavel", new[] { "Id" });
            DropPrimaryKey("dbo.Responsavel");
            DropPrimaryKey("dbo.Paciente");
            AddColumn("dbo.Responsavel", "Nome", c => c.String(nullable: false));
            AddColumn("dbo.Responsavel", "CpfCnpj", c => c.String(nullable: false, maxLength: 14));
            AddColumn("dbo.Responsavel", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Responsavel", "Cep", c => c.String(nullable: false));
            AddColumn("dbo.Responsavel", "Endereco", c => c.String());
            AddColumn("dbo.Responsavel", "Uf", c => c.Int(nullable: false));
            AddColumn("dbo.Responsavel", "Cidade", c => c.String(nullable: false));
            AddColumn("dbo.Responsavel", "fone", c => c.String());
            AddColumn("dbo.Responsavel", "Ip", c => c.String(nullable: false));
            AddColumn("dbo.Responsavel", "DataHora", c => c.DateTime(nullable: false));
            AddColumn("dbo.Paciente", "Nome", c => c.String(nullable: false));
            AddColumn("dbo.Paciente", "CpfCnpj", c => c.String(nullable: false, maxLength: 14));
            AddColumn("dbo.Paciente", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Paciente", "Cep", c => c.String(nullable: false));
            AddColumn("dbo.Paciente", "Endereco", c => c.String());
            AddColumn("dbo.Paciente", "Uf", c => c.Int(nullable: false));
            AddColumn("dbo.Paciente", "Cidade", c => c.String(nullable: false));
            AddColumn("dbo.Paciente", "fone", c => c.String());
            AddColumn("dbo.Paciente", "Ip", c => c.String(nullable: false));
            AddColumn("dbo.Paciente", "DataHora", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Responsavel", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Paciente", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Responsavel", "Id");
            AddPrimaryKey("dbo.Paciente", "Id");
            AddForeignKey("dbo.LogAcessoResponsavel", "Paciente_Id", "dbo.Paciente", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LogAcessoResponsavel", "Responsavel_Id", "dbo.Responsavel", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PermissaoResponsavelPaciente", "Paciente_Id", "dbo.Paciente", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Extrato", "Responsavel_Id", "dbo.Responsavel", "Id");
            AddForeignKey("dbo.Usuario", "Responsavel_Id", "dbo.Responsavel", "Id");
            AddForeignKey("dbo.Composicao", "Paciente_Id", "dbo.Paciente", "Id");
            AddForeignKey("dbo.Usuario", "Paciente_Id", "dbo.Paciente", "Id");
            DropTable("dbo.Pessoa");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Pessoa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
            
            DropForeignKey("dbo.Usuario", "Paciente_Id", "dbo.Paciente");
            DropForeignKey("dbo.Composicao", "Paciente_Id", "dbo.Paciente");
            DropForeignKey("dbo.Usuario", "Responsavel_Id", "dbo.Responsavel");
            DropForeignKey("dbo.Extrato", "Responsavel_Id", "dbo.Responsavel");
            DropForeignKey("dbo.PermissaoResponsavelPaciente", "Paciente_Id", "dbo.Paciente");
            DropForeignKey("dbo.LogAcessoResponsavel", "Responsavel_Id", "dbo.Responsavel");
            DropForeignKey("dbo.LogAcessoResponsavel", "Paciente_Id", "dbo.Paciente");
            DropPrimaryKey("dbo.Paciente");
            DropPrimaryKey("dbo.Responsavel");
            AlterColumn("dbo.Paciente", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Responsavel", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.Paciente", "DataHora");
            DropColumn("dbo.Paciente", "Ip");
            DropColumn("dbo.Paciente", "fone");
            DropColumn("dbo.Paciente", "Cidade");
            DropColumn("dbo.Paciente", "Uf");
            DropColumn("dbo.Paciente", "Endereco");
            DropColumn("dbo.Paciente", "Cep");
            DropColumn("dbo.Paciente", "Email");
            DropColumn("dbo.Paciente", "CpfCnpj");
            DropColumn("dbo.Paciente", "Nome");
            DropColumn("dbo.Responsavel", "DataHora");
            DropColumn("dbo.Responsavel", "Ip");
            DropColumn("dbo.Responsavel", "fone");
            DropColumn("dbo.Responsavel", "Cidade");
            DropColumn("dbo.Responsavel", "Uf");
            DropColumn("dbo.Responsavel", "Endereco");
            DropColumn("dbo.Responsavel", "Cep");
            DropColumn("dbo.Responsavel", "Email");
            DropColumn("dbo.Responsavel", "CpfCnpj");
            DropColumn("dbo.Responsavel", "Nome");
            AddPrimaryKey("dbo.Paciente", "Id");
            AddPrimaryKey("dbo.Responsavel", "Id");
            CreateIndex("dbo.Responsavel", "Id");
            CreateIndex("dbo.Paciente", "Id");
            AddForeignKey("dbo.Usuario", "Paciente_Id", "dbo.Paciente", "Id");
            AddForeignKey("dbo.Composicao", "Paciente_Id", "dbo.Paciente", "Id");
            AddForeignKey("dbo.Usuario", "Responsavel_Id", "dbo.Responsavel", "Id");
            AddForeignKey("dbo.Extrato", "Responsavel_Id", "dbo.Responsavel", "Id");
            AddForeignKey("dbo.PermissaoResponsavelPaciente", "Paciente_Id", "dbo.Paciente", "Id");
            AddForeignKey("dbo.LogAcessoResponsavel", "Responsavel_Id", "dbo.Responsavel", "Id");
            AddForeignKey("dbo.LogAcessoResponsavel", "Paciente_Id", "dbo.Paciente", "Id");
            AddForeignKey("dbo.Responsavel", "Id", "dbo.Pessoa", "Id");
            AddForeignKey("dbo.Paciente", "Id", "dbo.Pessoa", "Id");
        }
    }
}
