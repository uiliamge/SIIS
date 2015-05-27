using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIIS.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O {0} deve ter {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        public LoginProfissionalViewModel LoginProfissionalViewModel { get; set; }

        public LoginPacienteViewModel LoginPacienteViewModel { get; set; }
    }

    public class LoginProfissionalViewModel
    {
        [Display(Name = "Nº no Conselho Regional")]
        public int NumeroConselho { get; set; }

        [Display(Name = "Conselho Regional")]
        public string SiglaConselhoRegional { get; set; }
        
        [Display(Name = "UF")]
        public UfEnum UfConselhoRegional { get; set; }

        /// <summary>
        /// Concatena NumeroConselho + SiglaConselhoRegional + UfConselhoRegional.
        /// </summary>
        public string UserName
        {
            get
            {
                return NumeroConselho + SiglaConselhoRegional + UfConselhoRegional;
            }

        }
    }

    public class LoginPacienteViewModel
    {
        [Required]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }
        
    }

    public class RegisterProfissionalViewModel
    {
        [Required]
        [Display(Name = "Nome")]
        public string NomeCompleto { get; set; }

        [Required]
        [Display(Name = "Nº no Conselho Regional")]
        public int NumeroConselho { get; set; }

        [Required]
        [Display(Name = "Conselho Regional")]
        public string SiglaConselhoRegional { get; set; }

        [Required]
        [Display(Name = "UF")]
        public UfEnum UfConselhoRegional { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmação de Senha")]
        [Compare("Password", ErrorMessage = "A senha e a confirmação de senha não conferem.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Concatena NumeroConselho + SiglaConselhoRegional + UfConselhoRegional.
        /// </summary>
        public string userName
        {
            get
            {
                return NumeroConselho + SiglaConselhoRegional + UfConselhoRegional;
            }
        }
    }

    public class RegisterPacienteViewModel
    {
        [Required]
        [Display(Name = "Nome")]
        public string NomeCompleto { get; set; }

        [Required]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Tipo de Permissão")]
        public TipoPermissaoEnum TipoPermissao { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmação de Senha")]
        [Compare("Password", ErrorMessage = "A senha e a confirmação de senha não conferem.")]
        public string ConfirmPassword { get; set; }

        public PermissaoPacienteViewModel Permissao { get; set; }

        public string userName
        {
            get
            {
                return Cpf;
            }
        }
    }
    
    public class PermissaoPacienteViewModel
    {
        [Display(Name = "Nº no Conselho Regional")]
        public int NumeroConselho { get; set; }

        [Display(Name = "Conselho")]
        public string SiglaConselhoRegional { get; set; }

        [Display(Name = "UF")]
        public UfEnum UfConselhoRegional { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
