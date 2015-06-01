namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteracoesDeEndereco : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Responsavel", "Complemento", c => c.String());
            AddColumn("dbo.Paciente", "Complemento", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Paciente", "Complemento");
            DropColumn("dbo.Responsavel", "Complemento");
        }
    }
}
