using System.ComponentModel.DataAnnotations;

namespace EShop.WebUI.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "Ad")]
        [MaxLength(25)]
        [Required(ErrorMessage = "Ad Alanı Boş Bırakılamaz.")]
        public string FirstName { get; set; }
        [Display(Name = "Soyad")]
        [MaxLength(25)]
        [Required(ErrorMessage = "Soyad Alanı Boş Bırakılamaz.")]
        public string LastName { get; set; }
        [Display(Name = "EPosta")]
        [MaxLength(50)]
        [Required(ErrorMessage = "Email Alanı Boş Bırakılamaz.")]
        public string EMail { get; set; }
        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre Alanı Boş Bırakılamaz.")]

        public string Password { get; set; }
        [Display(Name = "Şifre Tekrarı")]
        [Required(ErrorMessage = "Şifre Tekrarı Alanı Boş Bırakılamaz.")]
        [Compare(nameof(Password), ErrorMessage = "Şifreler Eşlemiyor")]
        public string PasswordConfirm { get; set; }

    }
}
