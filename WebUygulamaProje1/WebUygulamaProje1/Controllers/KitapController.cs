using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{
	


    public class KitapController : Controller
    {
        private readonly IKitapRepository _kitapRepository; // obje oluşturdum.
		private readonly IKitapTuruRepository _kitapTuruRepository;
		public readonly IWebHostEnvironment _webHostEnvironment;

		public KitapController(IKitapRepository kitapRepository, IKitapTuruRepository kitapTuruRepository , IWebHostEnvironment webHostEnvironment )
		{
			_kitapRepository = kitapRepository;
			_kitapTuruRepository = kitapTuruRepository;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
        {
            List<Kitap> objKitapList = _kitapRepository.GetAll(includeProps:"kitapTuru").ToList();

            

            return View(objKitapList); // burda nesnemizi buranın içine yazarak modeldeki verilerimizi viewa gönderdik.
        }
        
        public IActionResult EkleGuncelle(int? id) 
        {
			IEnumerable<SelectListItem> KitapTuruList = _kitapTuruRepository.GetAll().Select(k => new SelectListItem
			{
				Text = k.Ad,
				Value = k.Id.ToString()
			});

			ViewBag.KitapTuruList = KitapTuruList;

			if(id==null || id == 0)
			{
				return View();
			}
			else
			{
				//güncelleme kısmı
				Kitap? kitapVT = _kitapRepository.Get(u => u.Id == id); //Expression<Func<T, bool>> filtre
																		// Veri Tabanından o id sahip kitap türünü bulup bize getirir.
				if (kitapVT == null)
				{
					return NotFound();
				}
				return View(kitapVT);
			}

        }

        [HttpPost]
		
        public IActionResult EkleGuncelle(Kitap kitap , IFormFile? file)
        {
			

            if (ModelState.IsValid) // Eğer kullanıcının girdiği değerde tüm koşullar sağlanıyorsa veri tabanına kaydederiz.
            {
				string wwwRootPath = _webHostEnvironment.WebRootPath;
				string kitapPath = Path.Combine(wwwRootPath, @"img");

				if(file != null)
				{
					using (var fileStream = new FileStream(Path.Combine(kitapPath, file.FileName), FileMode.Create))
					{
						file.CopyTo(fileStream);
					}

					kitap.ResimUrl = @"\img\" + file.FileName;
				}

				


				if (kitap.Id == 0)
				{
					_kitapRepository.Ekle(kitap);
					TempData["Basarili"] = "Yeni Kitap Başarıyla Oluşturuldu!";

				}
				else
				{
					_kitapRepository.Guncelle(kitap);
					TempData["Basarili"] = "Kitap Başarıyla Güncellendi!";

				}

				_kitapRepository.Kaydet();    
                return RedirectToAction("Index","Kitap");
            }
            return View();
        }

		/*
		public IActionResult Guncelle(int? id) //GÜNCELLEME İŞLEMİ. 
		{
            if (id==null || id==0)
            {
                return NotFound();
            }
            Kitap? kitapVT = _kitapRepository.Get(u=>u.Id==id); //Expression<Func<T, bool>> filtre
			// Veri Tabanından o id sahip kitap türünü bulup bize getirir.
			if (kitapVT == null)
            {
                return NotFound();
            }
			return View(kitapVT);
		}
		*/
		/*
		[HttpPost]  
		public IActionResult Guncelle(Kitap kitap)
		{
			if (ModelState.IsValid) // Eğer kullanıcının girdiği değerde tüm koşullar sağlanıyorsa veri tabanına kaydederiz.
			{
				_kitapRepository.Guncelle(kitap); // Update diyerek türün ismini güncellerim.
				_kitapRepository.Kaydet();    // savechanges yapmazsak bilgiler veri tabanına eklenmez!.
				TempData["Basarili"] = "Kitap Başarıyla Güncellendi!";

				return RedirectToAction("Index","Kitap");
			}
			return View();
		}
		*/

        //GET ACTİON 
		public IActionResult Sil(int? id) //SİLME İŞLEMİ. 
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Kitap? kitapVT = _kitapRepository.Get(u => u.Id == id); // Veri Tabanından o id sahip kitap türünü bulup bize getirir.
			if (kitapVT == null)
			{
				return NotFound();
			}
			return View(kitapVT);
		}


		[HttpPost,ActionName("Sil")]
		public IActionResult SilPOST(int? id)
		{
            Kitap? kitap = _kitapRepository.Get(u => u.Id == id);
            if (kitap == null)
            {
                return NotFound();
            }
			_kitapRepository.Sil(kitap); // Remove Kaldır..
			_kitapRepository.Kaydet();
			TempData["Basarili"] = "Kitap Silme İşlemi Başarılı!";

			return RedirectToAction("Index", "Kitap");
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
