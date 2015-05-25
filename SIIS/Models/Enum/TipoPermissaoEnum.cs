using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIIS.Models
{
    public enum TipoPermissaoEnum
    {
        [Description("Qualquer Profissional")]
        QualquerProfissional = 1,
        [Description("Escolher quem pode acessar")]
        EscolherQuemPodeAcessar = 2
    }
}
