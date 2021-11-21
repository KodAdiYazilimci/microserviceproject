using System.ComponentModel.DataAnnotations;

namespace Presentation.UI.Web.Identity.Models
{
    public class SignInModel
    {
        [Display(Name = "E-posta:")]
        [Required(ErrorMessage = "E-posta adresi gerekli")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi")]
        public string Email { get; set; }

        [Display(Name = "Parola:")]
        [Required(ErrorMessage = "Parola gerekli")]
        public string Password { get; set; }
    }
}
