using System.ComponentModel;

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
