using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products_Management.Data;
using Products_Management.Models;

namespace Products_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public ProductsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var allProducts = dbContext.Products.ToList();
            return Ok(allProducts);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetProductById(int id)
        {
            var product = dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult InsertProduct(AddProductsDto addProductsDto)
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

            return Ok(productsEntity);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateProduct(int id, UpdateProductsDto updateProductsDto)
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

            return Ok(product);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = dbContext.Products.Find(id);
            if(product is null)
            {
                return NotFound();
            }
            dbContext.Products.Remove(product);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}