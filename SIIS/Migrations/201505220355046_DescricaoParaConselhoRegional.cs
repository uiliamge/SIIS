using System.Data.Entity.Migrations;

namespace SIIS.Migrations
{
    public partial class DescricaoParaConselhoRegional : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConselhoRegional", "Descricao", c => c.String());
            AlterColumn("dbo.ConselhoRegional", "Sigla", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ConselhoRegional", "Sigla", c => c.String());
            DropColumn("dbo.ConselhoRegional", "Descricao");
        }
    }
}
