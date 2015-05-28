using System.Data.Entity.Migrations;

namespace SIIS.Migrations.Identity
{
    public partial class NomeCompletoNoApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "NomeCompleto", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "NomeCompleto");
        }
    }
}
