using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq.Expressions;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{
    

    public class KitapTuruController : Controller
    {
        private readonly IKitapTuruRepository _kitapTuruRepository;

        public KitapTuruController(IKitapTuruRepository context)
        {
			_kitapTuruRepository = context;
        }

        public IActionResult Index()
        {
            List<KitapTuru> objKitapTuruList = _kitapTuruRepository.GetAll().ToList();
            return View(objKitapTuruList); // burda nesnemizi buranın içine yazarak modeldeki verilerimizi viewa gönderdik.
        }
        
        public IActionResult Ekle() // burda ise Kitap türü oluştur buttonu için action oluşturdum,ana sayfada tıkladığımız her yapı için gereklidir.
        {
            return View();
        }

        [HttpPost]  // Kullanıcının girdiği Yeni kitap türlerini veri tabanına ekledim.
        public IActionResult Ekle(KitapTuru kitapTuru)
        {
            if (ModelState.IsValid) // Eğer kullanıcının girdiği değerde tüm koşullar sağlanıyorsa veri tabanına kaydederiz.
            {
				_kitapTuruRepository.Ekle(kitapTuru); //KitapTurleri veritabanımdaki tablomun adı.
				_kitapTuruRepository.Kaydet();    // savechanges yapmazsak bilgiler veri tabanına eklenmez!.
                TempData["Basarili"] = "Yeni Kitap Türü Başarıyla Oluşturuldu!";
                return RedirectToAction("Index");
            }
            return View();
        }

		public IActionResult Guncelle(int? id) //GÜNCELLEME İŞLEMİ. 
		{
            if (id==null || id==0)
            {
                return NotFound();
            }
            KitapTuru? kitapTuruVT = _kitapTuruRepository.Get(u=>u.Id==id); //Expression<Func<T, bool>> filtre
			// Veri Tabanından o id sahip kitap türünü bulup bize getirir.
			if (kitapTuruVT == null)
            {
                return NotFound();
            }
			return View(kitapTuruVT);
		}

		[HttpPost]  
		public IActionResult Guncelle(KitapTuru kitapTuru)
		{
			if (ModelState.IsValid) // Eğer kullanıcının girdiği değerde tüm koşullar sağlanıyorsa veri tabanına kaydederiz.
			{
				_kitapTuruRepository.Guncelle(kitapTuru); // Update diyerek türün ismini güncellerim.
				_kitapTuruRepository.Kaydet();    // savechanges yapmazsak bilgiler veri tabanına eklenmez!.
				TempData["Basarili"] = "Kitap Türü Başarıyla Güncellendi!";

				return RedirectToAction("Index","KitapTuru");
			}
			return View();
		}


        //GET ACTİON 
		public IActionResult Sil(int? id) //SİLME İŞLEMİ. 
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			KitapTuru? kitapTuruVT = _kitapTuruRepository.Get(u => u.Id == id); // Veri Tabanından o id sahip kitap türünü bulup bize getirir.
			if (kitapTuruVT == null)
			{
				return NotFound();
			}
			return View(kitapTuruVT);
		}


		[HttpPost,ActionName("Sil")]
		public IActionResult SilPOST(int? id)
		{
            KitapTuru? kitapTuru = _kitapTuruRepository.Get(u => u.Id == id);
            if (kitapTuru == null)
            {
                return NotFound();
            }
			_kitapTuruRepository.Sil(kitapTuru); // Remove Kaldır..
			_kitapTuruRepository.Kaydet();
			TempData["Basarili"] = "Kitap Silme İşlemi Başarılı!";

			return RedirectToAction("Index", "KitapTuru");
		}


		public IActionResult Populer()
        {
            return View();
        }
        public IActionResult Odul()
        {
            return View();
        }
        public IActionResult Iletisim()
        {
            return View();
        }
       
    }
}
