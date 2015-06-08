namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class indiceDaComposicaoNaSecao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Secao", "IndiceComposicao", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Secao", "IndiceComposicao");
        }
    }
}
