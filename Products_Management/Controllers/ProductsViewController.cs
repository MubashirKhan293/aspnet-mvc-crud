using Microsoft.AspNetCore.Mvc;
using Products_Management.Data;
using Products_Management.Models;
using System.Linq;

namespace Products_Management.Controllers
{
    public class ProductsViewController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ProductsViewController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var allProducts = dbContext.Products.ToList();
            return View(allProducts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AddProductsDto addProductsDto)
        {
            if (ModelState.IsValid)
            {
                var productsEntity = new Models.Entity.Product()
                {
                    Name = addProductsDto.Name,
                    Description = addProductsDto.Description,
                    Price = addProductsDto.Price,
                    StockQuantity = addProductsDto.StockQuantity
                };
                dbContext.Products.Add(productsEntity);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(addProductsDto);
        }

        public IActionResult Edit(Guid id)
        {
            var product = dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            var updateProductsDto = new UpdateProductsDto
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            };
            return View(updateProductsDto);
        }

        [HttpPost]
        public IActionResult Edit(Guid id, UpdateProductsDto updateProductsDto)
        {
            if (ModelState.IsValid)
            {
                var product = dbContext.Products.Find(id);
                if (product == null)
                {
                    return NotFound();
                }
                product.Name = updateProductsDto.Name;
                product.Description = updateProductsDto.Description;
                product.Price = updateProductsDto.Price;
                product.StockQuantity = updateProductsDto.StockQuantity;

                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(updateProductsDto);
        }

        public IActionResult Delete(Guid id)
        {
            var product = dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var product = dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            dbContext.Products.Remove(product);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
