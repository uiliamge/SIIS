using System.Data.Entity.Migrations;
using SIIS.Models;

namespace SIIS.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SiteDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SiteDataContext context)
        {
            //context.ConselhosRegionais.AddOrUpdate(
            //      p => p.Sigla,
            //      new ConselhoRegional { Sigla = "CRM" },
            //      new ConselhoRegional { Sigla = "CRO" },
            //      new ConselhoRegional { Sigla = "CRF" }
            //    );
        }
    }
}
