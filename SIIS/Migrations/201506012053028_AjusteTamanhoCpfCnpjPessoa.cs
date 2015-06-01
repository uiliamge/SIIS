namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteTamanhoCpfCnpjPessoa : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Responsavel", "CpfCnpj", c => c.String(nullable: false, maxLength: 18));
            AlterColumn("dbo.Paciente", "CpfCnpj", c => c.String(nullable: false, maxLength: 18));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Paciente", "CpfCnpj", c => c.String(nullable: false, maxLength: 14));
            AlterColumn("dbo.Responsavel", "CpfCnpj", c => c.String(nullable: false, maxLength: 14));
        }
    }
}
