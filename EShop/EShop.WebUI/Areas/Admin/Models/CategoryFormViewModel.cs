using System.ComponentModel.DataAnnotations;

namespace EShop.WebUI.Areas.Admin.Models
{
    public class CategoryFormViewModel
    {
        //bu model ile aynı formdan hem ekleme hem güncelleme yapılacağı için Id'leride tanımlıyoruz.

        public int Id { get; set; }
        [Display(Name= "Kategori Adı")]
        [Required(ErrorMessage ="Kategori Adı Alanı Doldurmak Zorunludur.")]
        public string  Name { get; set; }

        [Display(Name ="Açıklama")]
        [MaxLength(100)]
        public string? Description { get; set; }
    }
}
