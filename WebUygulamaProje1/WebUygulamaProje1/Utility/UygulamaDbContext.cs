using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebUygulamaProje1.Models;


namespace WebUygulamaProje1.Utility
{      // Veri Tabanına Tablo oluşturmak için buraya eklerim.
    public class UygulamaDbContext : IdentityDbContext  // burda veri tabanıyla entities sınıfıyla köprü kurduk.
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options) { }
        
        public DbSet<KitapTuru> KitapTurleri { get; set; }    // Burada tablomu oluşturup adını verdim.

        public DbSet<Kitap> Kitaplar { get; set; } // Yeni tablomu oluşturup Kitaplar ismini verdim.

        public DbSet<Kiralama> Kiralamalar { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

		

	}
}
