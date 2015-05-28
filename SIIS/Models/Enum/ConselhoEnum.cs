using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIIS.Models
{
    public enum ConselhoEnum
    {
        [Description("Conselho Regional de Medicina")]
        CRM = 1,
        [Description("Conselho Regional de Odontologia")]
        CRO = 2,
        [Description("Conselho Regional de Fisioterapia")]
        CRF = 3,
        [Description("Conselho Regional de Nutricionistas")]
        CRN = 4,
        [Description("Conselho Regional de Psicologia")]
        CRP = 5
    }
}
