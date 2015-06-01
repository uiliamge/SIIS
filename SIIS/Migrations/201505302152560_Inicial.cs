namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Composicao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(maxLength: 100),
                        Extrato_Id = c.Int(),
                        Paciente_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Extrato", t => t.Extrato_Id)
                .ForeignKey("dbo.Paciente", t => t.Paciente_Id)
                .Index(t => t.Extrato_Id)
                .Index(t => t.Paciente_Id);
            
            CreateTable(
                "dbo.Extrato",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomeDoArquivo = c.String(maxLength: 100),
                        IndImportado = c.Byte(nullable: false),
                        DataHora = c.DateTime(nullable: false),
                        DataReferencia = c.DateTime(nullable: false),
                        IP = c.String(nullable: false),
                        Responsavel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Responsavel", t => t.Responsavel_Id)
                .Index(t => t.Responsavel_Id);
            
            CreateTable(
                "dbo.Responsavel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumeroConselhoRegional = c.Int(nullable: false),
                        ConselhoRegional = c.Int(nullable: false),
                        UfConselhoRegional = c.Int(nullable: false),
                        UserId = c.String(),
                        CpfCnpj = c.String(nullable: false, maxLength: 14),
                        Nome = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Cep = c.String(nullable: false),
                        Endereco = c.String(),
                        Bairro = c.String(),
                        NumeroEndereco = c.String(),
                        Uf = c.Int(nullable: false),
                        Cidade = c.String(nullable: false),
                        Telefone = c.String(),
                        Ip = c.String(nullable: false),
                        DataHora = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Secao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 100),
                        Composicao_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Composicao", t => t.Composicao_Id)
                .Index(t => t.Composicao_Id);
            
            CreateTable(
                "dbo.ConselhoRegional",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sigla = c.String(nullable: false),
                        Descricao = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Entrada",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(maxLength: 2000),
                        Secao_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Secao", t => t.Secao_Id)
                .Index(t => t.Secao_Id);
            
            CreateTable(
                "dbo.LogAcessoResponsavel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataHora = c.DateTime(nullable: false),
                        Ip = c.String(nullable: false),
                        Paciente_Id = c.Int(nullable: false),
                        Responsavel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Paciente", t => t.Paciente_Id, cascadeDelete: true)
                .ForeignKey("dbo.Responsavel", t => t.Responsavel_Id, cascadeDelete: true)
                .Index(t => t.Paciente_Id)
                .Index(t => t.Responsavel_Id);
            
            CreateTable(
                "dbo.Paciente",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataNascimento = c.DateTime(nullable: false),
                        TipoPermissao = c.Int(nullable: false),
                        UserId = c.String(),
                        CpfCnpj = c.String(nullable: false, maxLength: 14),
                        Nome = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Cep = c.String(nullable: false),
                        Endereco = c.String(),
                        Bairro = c.String(),
                        NumeroEndereco = c.String(),
                        Uf = c.Int(nullable: false),
                        Cidade = c.String(nullable: false),
                        Telefone = c.String(),
                        Ip = c.String(nullable: false),
                        DataHora = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PermissaoResponsavelPaciente",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumeroConselho = c.Int(nullable: false),
                        SiglaConselhoRegional = c.Int(nullable: false),
                        UfConselhoRegional = c.Int(nullable: false),
                        Paciente_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Paciente", t => t.Paciente_Id, cascadeDelete: true)
                .Index(t => t.Paciente_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogAcessoResponsavel", "Responsavel_Id", "dbo.Responsavel");
            DropForeignKey("dbo.LogAcessoResponsavel", "Paciente_Id", "dbo.Paciente");
            DropForeignKey("dbo.PermissaoResponsavelPaciente", "Paciente_Id", "dbo.Paciente");
            DropForeignKey("dbo.Composicao", "Paciente_Id", "dbo.Paciente");
            DropForeignKey("dbo.Entrada", "Secao_Id", "dbo.Secao");
            DropForeignKey("dbo.Secao", "Composicao_Id", "dbo.Composicao");
            DropForeignKey("dbo.Extrato", "Responsavel_Id", "dbo.Responsavel");
            DropForeignKey("dbo.Composicao", "Extrato_Id", "dbo.Extrato");
            DropIndex("dbo.PermissaoResponsavelPaciente", new[] { "Paciente_Id" });
            DropIndex("dbo.LogAcessoResponsavel", new[] { "Responsavel_Id" });
            DropIndex("dbo.LogAcessoResponsavel", new[] { "Paciente_Id" });
            DropIndex("dbo.Entrada", new[] { "Secao_Id" });
            DropIndex("dbo.Secao", new[] { "Composicao_Id" });
            DropIndex("dbo.Extrato", new[] { "Responsavel_Id" });
            DropIndex("dbo.Composicao", new[] { "Paciente_Id" });
            DropIndex("dbo.Composicao", new[] { "Extrato_Id" });
            DropTable("dbo.PermissaoResponsavelPaciente");
            DropTable("dbo.Paciente");
            DropTable("dbo.LogAcessoResponsavel");
            DropTable("dbo.Entrada");
            DropTable("dbo.ConselhoRegional");
            DropTable("dbo.Secao");
            DropTable("dbo.Responsavel");
            DropTable("dbo.Extrato");
            DropTable("dbo.Composicao");
        }
    }
}
