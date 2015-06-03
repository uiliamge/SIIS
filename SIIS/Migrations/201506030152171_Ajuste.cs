namespace SIIS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ajuste : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Extrato", "Paciente_Id", c => c.Int());
            CreateIndex("dbo.Extrato", "Paciente_Id");
            AddForeignKey("dbo.Extrato", "Paciente_Id", "dbo.Paciente", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Extrato", "Paciente_Id", "dbo.Paciente");
            DropIndex("dbo.Extrato", new[] { "Paciente_Id" });
            DropColumn("dbo.Extrato", "Paciente_Id");
        }
    }
}
