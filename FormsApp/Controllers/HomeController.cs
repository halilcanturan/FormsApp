using FormsApp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace FormsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController()
        {
        }

        public IActionResult Index(string searchString, string category)
        {
            var products = Repository.Product;

            if (!String.IsNullOrEmpty(searchString)) //navbardaki search
            {
                ViewBag.SearchString = searchString;
                products = products.Where(p => p.Name!.ToLower().Contains(searchString)).ToList();
            }

            if (!String.IsNullOrEmpty(category) && category != "0") //ana ekranda filtreleme
            {
                products = products.Where(p => p.CategoryId == int.Parse(category)).ToList();
            }

            //CategoryId'lerine göre Name'leri çekip, ViewBag e atıyoruz çünkü Index de selecList oluşturup kullanıyoruz.
            //ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name",category);

            var model = new ProductViewModel
            {
                Products = products,
                Categories = Repository.Categories,
                SelectedCategory = category
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product model, IFormFile imageFile)
        {
            var extension = "";

            if (imageFile != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                extension = Path.GetExtension(imageFile.FileName);//yüklenecek doyanın uzantısını alır
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("", "Geçerli bir resim seçiniz");
                }
            }

            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName); //yüklenecek resmin yolu ayarlanır
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    model.Image = randomFileName; //modelin image bilgisi ile yukarıda oluşan random filename'ı ilişkilendirdik.
                    model.ProductId = Repository.Product.Count + 1;
                    Repository.CreateProduct(model);
                    return RedirectToAction("Index");
                }

            }
            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = Repository.Product.FirstOrDefault(x => x.ProductId == id);
            if (entity == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product model, IFormFile? imageFile)
        {
            if (id != model.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (imageFile != null)
                {
                    var extension = Path.GetExtension(imageFile.FileName);//yüklenecek doyanın uzantısını alır
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName); //yüklenecek resmin yolu ayarlanır

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    model.Image = randomFileName;
                }
                Repository.EditProduct(model);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var entity = Repository.Product.FirstOrDefault(p => p.ProductId == id);

            if (entity == null)
            {
                return NotFound();
            }

            Repository.DeleteProduct(entity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditProducts (List<Product> Products)
        {
            foreach (var product in Products)
            {
                Repository.EditIsActive(product);
            }
            return RedirectToAction("Index");
        }

    }
}