using System.ComponentModel.DataAnnotations;

namespace Presentation.UI.Web.Models
{
    public class CredentialModel
    {
        [Display(Name = "E-posta:")]
        [Required(ErrorMessage = "E-posta adresi gerekli")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi")]
        public string Email { get; set; }

        [Display(Name = "Parola:")]
        [Required(ErrorMessage = "Parola gerekli")]
        public string Password { get; set; }

        [Display(Name = "Parola Tekrar:")]
        [Required(ErrorMessage = "Parola tekrar etmeli")]
        [Compare(nameof(Password), ErrorMessage = "Parola aynı olmalı")]
        public string PasswordRepeat { get; set; }
    }
}
