using System;
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
        [Display(Name = "Senha Atual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} deve possuir ao menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmação da Nova Senha")]
        [Compare("NewPassword", ErrorMessage = "A senha e a confirmação de senha não conferem.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
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
        public ConselhoEnum SiglaConselhoRegional { get; set; }
        
        [Display(Name = "UF")]
        public UfEnum UfConselhoRegional { get; set; }

        /// <summary>
        /// Concatena NumeroConselho + SiglaConselhoRegional + UfConselhoRegional.
        /// </summary>
        public string UserName
        {
            get
            {
                return NumeroConselho.ToString() + SiglaConselhoRegional + UfConselhoRegional;
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
        public ConselhoEnum SiglaConselhoRegional { get; set; }

        [Required]
        [Display(Name = "UF")]
        public UfEnum UfConselhoRegional { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "CPF/CNPJ")]
        public string CpfCnpj { get; set; }

        [Required]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [Display(Name = "CEP")]
        public string Cep { get; set; }

        [Display(Name = "Logradouro")]
        public string Endereco { get; set; }

        [Display(Name = "Nº")]
        public string NumeroEndereco { get; set; }

        [Display(Name = "Complemento")]
        public string Complemento { get; set; }

        [Display(Name = "Bairro")]
        public string Bairro { get; set; }

        [Display(Name = "UF")]
        public UfEnum Uf { get; set; }

        [Required]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; }

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
                return NumeroConselho + SiglaConselhoRegional.ToString() + UfConselhoRegional.ToString();
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
        [Display(Name = "Data de Nascimento")]
        public string DataNascimento { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [Display(Name = "CEP")]
        public string Cep { get; set; }

        [Display(Name = "Logradouro")]
        public string Endereco { get; set; }

        [Display(Name = "Nº")]
        public string NumeroEndereco { get; set; }

        [Display(Name = "Complemento")]
        public string Complemento { get; set; }

        [Display(Name = "Bairro")]
        public string Bairro { get; set; }

        [Display(Name = "UF")]
        public UfEnum Uf { get; set; }

        [Required]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; }

        [Display(Name = "Telefone")]
        public string fone { get; set; }

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

        public List<PermissaoPacienteViewModel> Permissao { get; set; }

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
        public ConselhoEnum SiglaConselhoRegional { get; set; }

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
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmação de Senha")]
        [Compare("Password", ErrorMessage = "A senha e a confirmação de senha não conferem.")]
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
