using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Linq.Expressions;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{
   


    public class KiralamaController : Controller
    {
        private readonly IKiralamaRepository _kiralamaRepository; // obje oluşturdum.
		private readonly IKitapRepository _kitapRepository;
		public readonly IWebHostEnvironment _webHostEnvironment;

		public KiralamaController(IKiralamaRepository kiralamaRepository, IKitapRepository kitapRepository , IWebHostEnvironment webHostEnvironment )
		{
			_kiralamaRepository = kiralamaRepository;
			_kitapRepository = kitapRepository;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
        {
            List<Kiralama> objKiralamaList = _kiralamaRepository.GetAll(includeProps:"Kitap").ToList();
            return View(objKiralamaList); // burda nesnemizi buranın içine yazarak modeldeki verilerimizi viewa gönderdik.
        }



        //GET
        public IActionResult EkleGuncelle(int? id)
			{
				IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll().Select(k => new SelectListItem
				{
					Text = k.KitapAdi,
					Value = k.Id.ToString()
				});

				ViewBag.KitapList = KitapList;

				if (id == null || id == 0)
				{
					return View();
				}
				else
				{
					//güncelleme kısmı
					Kiralama? kiralamaVT = _kiralamaRepository.Get(u => u.Id == id); //Expression<Func<T, bool>> filtre
																					 // Veri Tabanından o id sahip kitap türünü bulup bize getirir.
					if (kiralamaVT == null)
					{
						return NotFound();
					}
					return View(kiralamaVT);
				}

			}
		
        [HttpPost]
        public IActionResult EkleGuncelle(Kiralama kiralama)
        {
			

            if (ModelState.IsValid) // Eğer kullanıcının girdiği değerde tüm koşullar sağlanıyorsa veri tabanına kaydederiz.
            {
				


				if (kiralama.Id == 0)
				{
					_kiralamaRepository.Ekle(kiralama);
					TempData["Basarili"] = "Yeni Kiralama İşlemi Başarıyla Oluşturuldu!";

				}
				else
				{
					_kiralamaRepository.Guncelle(kiralama);
					TempData["Basarili"] = "Kiralama Kayıt Güncelleme Başarılı!";

				}

				_kiralamaRepository.Kaydet();    
                return RedirectToAction("Index","Kiralama");
            }
            return View();
        }



        //GET ACTİON 
        public IActionResult Sil(int? id) //SİLME İŞLEMİ. 
		{

			IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll().Select(k => new SelectListItem
			{
				Text = k.KitapAdi,
				Value = k.Id.ToString()
			});

			ViewBag.KitapList = KitapList;



			if (id == null || id == 0)
			{
				return NotFound();
			}
			Kiralama? kiralamaVT = _kiralamaRepository.Get(u => u.Id == id); // Veri Tabanından o id sahip kitap türünü bulup bize getirir.
			if (kiralamaVT == null)
			{
				return NotFound();
			}
			return View(kiralamaVT);
		}

       
        [HttpPost,ActionName("Sil")]
		public IActionResult SilPOST(int? id)
		{
            Kiralama? kiralama = _kiralamaRepository.Get(u => u.Id == id);
            if (kiralama == null)
            {
                return NotFound();
            }
			_kiralamaRepository.Sil(kiralama); // Remove Kaldır..
			_kiralamaRepository.Kaydet();
			TempData["Basarili"] = "Kayıt Silme İşlemi Başarılı!";

			return RedirectToAction("Index", "Kiralama");
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
