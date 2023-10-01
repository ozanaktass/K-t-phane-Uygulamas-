using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebUygulamaProje1.Models
{
    public class KitapTuru
    {
        [Key]   // kitap ıd lerinin birbirinden farklı olması lazım bunun için primary key kullandım.
        public int Id { get; set; }



        [Required (ErrorMessage ="Kitap Türü Adı Boş Bırakılamaz!")]  // kitap adının not null, boş olmaması için required kullandım.
        [DisplayName("Kitap Türü Adı")]
        [MaxLength(25)]
        public string Ad { get; set; }


    }
}
