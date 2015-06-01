namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteData : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Paciente", "DataNascimento", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Paciente", "DataNascimento", c => c.DateTime(nullable: false));
        }
    }
}
