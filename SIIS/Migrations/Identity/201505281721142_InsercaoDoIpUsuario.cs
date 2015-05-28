namespace SIIS.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsercaoDoIpUsuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Ip", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Ip");
        }
    }
}
