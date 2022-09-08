using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntitieFramework
{
    public class ProductDal : EfEntityRepositoryBase<Product, AppDbContext>, IProductDal
    {
     
        public  Product AddProduct(Product product)
        {
            using AppDbContext _context = new();
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public List<Product> GetAllHomeProducts()
        {
            using AppDbContext _context = new();
            return _context.Products.Where(x =>x.IsDelete == false).Take(3).ToList();
        }
        public List<Product> GetByCategory(int categoryId)
        {
            using AppDbContext _context = new();
            var productsCategories = _context.ProductCategories.Where(x => x.CategoryId == categoryId).ToList();

            List<Product> products = new();

            foreach (var productCategory in productsCategories)
            {
                var findedProduct = _context.Products.FirstOrDefault(x => x.Id == productCategory.Id);
                products.Add(findedProduct);
            }
            return products;
        }

        public List<Product> GetFilterShopProduct(int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            using var _context = new AppDbContext();
            if(categoryId != null)
            {
                var productCategory = _context.ProductCategories.Where(x => x.CategoryId == categoryId).ToList();
                List<Product> result = new();
                for (int i = 0; i < productCategory.Count; i++)
                {
                    var product = _context.Products.FirstOrDefault(x => x.Id == productCategory[i].ProductId);
                    result.Add(product);
                }
                return result;
            }

            var pro = _context.Products.ToList();
            return pro;
        }

        public ProductDetailDTO GetProductById(int productId)
        {
            using AppDbContext _context = new();
            var productCategory = _context.ProductCategories.Include(x =>x.Category).Where(x => x.ProductId == productId).ToList();
            var product = _context.Products.FirstOrDefault(x => x.Id == productId);

            List<Category> categoryList = new();
            foreach(var item in productCategory)
            {
                categoryList.Add(item.Category);
            }
            ProductDetailDTO result = new()
            {
                Id = product.Id,
                Name = product.Name,
                Categories = categoryList,
                CoverPhoto = product.CoverPhoto,
                Description = product.Description,
                Discound = product.Discound,
                PhotoURL = product.PhotoURL,
                Price = product.Price,
                Quantity = product.Quantity,
            };

            return result;
        }
        

        public List<Product> RelatedProducts(List<int> categoriesId, int productId)
        {
            using var _context = new AppDbContext();
            var productCategories = _context.ProductCategories.Where(x =>x.CategoryId == categoriesId[0]).Include(x =>x.Product);
            List<Product> products = new();
            for (int i = 0; i < productCategories.ToList().Count; i++)
            {
                products.Add(productCategories.Skip(i).FirstOrDefault().Product);
            }
            return products.Where(x =>x.Id !=productId).Take(3).ToList();
        }
    }
}
