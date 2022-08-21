using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Dashboard.ViewModel;

namespace WebUI.Areas.Dashboard.Controllers
{
    [Area("dashboard")]
    public class ProductController : Controller
    {
        private readonly IProductManager _productManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IProductCategoryManager _productCategoryManager;

        public ProductController(IProductManager productManager, ICategoryManager categoryManager, IProductCategoryManager productCategoryManager)
        {
            _productManager = productManager;
            _categoryManager = categoryManager;
            _productCategoryManager = productCategoryManager;
        }

        public IActionResult Index()
        {
            var products = _productManager.GetAll();
            return View(products);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Categories"] = _categoryManager.GetAll();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile NewPhotoURL, IFormFile NewCoverPhoto, List<int> Categories)
        {
           
            string myPhoto = Guid.NewGuid().ToString() + Path.GetExtension(NewPhotoURL.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", myPhoto);
            using(var stream = new FileStream(path, FileMode.Create))
            {
                NewPhotoURL.CopyTo(stream);
            }

            string myCoverPhoto = Guid.NewGuid().ToString() + Path.GetExtension(NewCoverPhoto.FileName);
            string pathCoverPhoto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", myCoverPhoto);
            using (var stream = new FileStream(pathCoverPhoto, FileMode.Create))
            {
                NewCoverPhoto.CopyTo(stream);
            }

            product.PhotoURL = "/image/" + myPhoto;
            product.CoverPhoto = "/image/" + myCoverPhoto;
            product.SeoUrl = "test";
            var products = _productManager.Add(product);

            for(int i=0; i < Categories.Count; i++)
            {
                ProductCategory productCategory = new()
                {
                    CategoryId = Categories[i],
                    ProductId = products.Id
                };
                _productCategoryManager.AddProductCategory(productCategory);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _productManager.Get(id);
            if (product == null)
                return NotFound();
            return View(product);
        }
        [HttpPost]
        public IActionResult Delete(Product product)
        {
            try
            {
                _productManager.Delete(product.Id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return NotFound();
            }
            

        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var product = _productManager.Get(id);
            var categories = _categoryManager.GetAll();
            var productCategories = _productCategoryManager.GetProductCategoriesById(id);

            List<int> productCategoryId = new();
            foreach(var pc in productCategories)
            {
                productCategoryId.Add(pc.CategoryId);
            }
            ProductVM vm = new()
            {
                Product = product,
                Categories = categories,
                ProductCategories = productCategoryId,
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult Update(Product product, List<int> CategoriesId, IFormFile NewPhotoUrl, IFormFile NewCoverPhoto)
        {
            try
            {
                if(NewPhotoUrl != null)
                {
                    string myPhoto = Guid.NewGuid().ToString() + Path.GetExtension(NewPhotoUrl.FileName);
                    string pathCoverPhoto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", myPhoto);
                    using (var stream = new FileStream(pathCoverPhoto, FileMode.Create))
                    {
                        NewPhotoUrl.CopyTo(stream);
                    }

                    product.PhotoURL = "/image/" + myPhoto;
                }
                if(NewCoverPhoto != null)
                {
                    string myPhoto = Guid.NewGuid().ToString() + Path.GetExtension(NewCoverPhoto.FileName);
                    string pathCoverPhoto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", myPhoto);
                    using (var stream = new FileStream(pathCoverPhoto, FileMode.Create))
                    {
                        NewCoverPhoto.CopyTo(stream);
                    }

                    product.CoverPhoto = "/image/" + myPhoto;
                }
                _productCategoryManager.RemoveProductCategories(product.Id);
                for (int i = 0; i < CategoriesId.Count; i++)
                {
                    ProductCategory productCategory = new()
                    {
                        CategoryId = CategoriesId[i],
                        ProductId = product.Id
                    };
                    _productCategoryManager.AddProductCategory(productCategory);
                }
                product.SeoUrl = "test";
                _productManager.Update(product);
                return RedirectToAction("Index");   

            }
            catch (Exception)
            {
                return View();
            }
           
        }
       
    }
}
