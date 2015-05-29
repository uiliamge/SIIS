namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColoqueiTipoPermissaonoPaciente : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Responsavel", "ConselhoRegional_Id", "dbo.ConselhoRegional");
            DropIndex("dbo.Responsavel", new[] { "ConselhoRegional_Id" });
            AddColumn("dbo.Responsavel", "ConselhoRegional", c => c.Int(nullable: false));
            AddColumn("dbo.Paciente", "TipoPermissao", c => c.Int(nullable: false));
            DropColumn("dbo.Responsavel", "ConselhoRegional_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Responsavel", "ConselhoRegional_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Paciente", "TipoPermissao");
            DropColumn("dbo.Responsavel", "ConselhoRegional");
            CreateIndex("dbo.Responsavel", "ConselhoRegional_Id");
            AddForeignKey("dbo.Responsavel", "ConselhoRegional_Id", "dbo.ConselhoRegional", "Id", cascadeDelete: true);
        }
    }
}
