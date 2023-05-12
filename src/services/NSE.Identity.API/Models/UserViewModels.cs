using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NSE.Identity.API.Models
{
    public class UserIdentityRegister
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "As senhas não conferem.")]
        public string PasswordConfirmation { get; set; }
    }

    public class UserIdentityLogin
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }
    }

    public class UserIdentityResponseLogin
    {
        public string AccessToken { get; set; }
        public Guid RefreshToken { get; set; }
        public double ExpiresIn { get; set; }
        public UserIdentityToken UserToken { get; set; }
    }

    public class UserIdentityToken
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserIdentityClaim> Claims { get; set; }
    }

    public class UserIdentityClaim
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
}