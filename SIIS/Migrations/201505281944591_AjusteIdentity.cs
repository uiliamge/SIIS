namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteIdentity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Paciente", "Id", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
        }
    }
}
