﻿using Microsoft.Owin;
using Owin;
using SIIS;

[assembly: OwinStartup(typeof(Startup))]
namespace SIIS
{
    public partial class Startup
    {
        // The OwinStartupAttribute above is responsible for having
        // the method below being invoked almost immediately after 
        // Application_Start in Global.asax.cs
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
