using SIIS.Models;

namespace SIIS.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SIIS.Models.SiteDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SIIS.Models.SiteDataContext context)
        {
            context.ConselhosRegionais.AddOrUpdate(
                  p => p.Sigla,
                  new ConselhoRegional { Sigla = "CRM" },
                  new ConselhoRegional { Sigla = "CRO" },
                  new ConselhoRegional { Sigla = "CRF" }
                );
        }
    }
}
