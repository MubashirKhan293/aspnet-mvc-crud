using Microsoft.AspNetCore.Mvc;
using Moq;
using Products_Management.Controllers;
using Products_Management.Data;
using Products_Management.Models.Entity;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace Products_Management.Tests
{
    public class ProductsControllerTests
    {
        private readonly ProductsController _controller;
        private readonly ApplicationDbContext _context;

        public ProductsControllerTests()
        {
            // Set up in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestDb")
                            .Options;

            _context = new ApplicationDbContext(options);
            _controller = new ProductsController(_context);

            // Seed the in-memory database
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _context.Products.AddRange(new List<Product>
            {
                new Product { Id = 1, Name = "Product1", Description = "Description1", Price = 10.0M, StockQuantity = 100 },
                new Product { Id = 2, Name = "Product2", Description = "Description2", Price = 20.0M, StockQuantity = 200 },
            });
            _context.SaveChanges();
        }

        [Fact]
        public void GetAllProducts_ReturnsOkResult()
        {
            // Act
            var result = _controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(2, products.Count);
        }

        [Fact]
        public void GetProductById_UnknownId_ReturnsNotFoundResult()
        {
            // Act
            var result = _controller.GetProductById(3);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetProductById_ExistingId_ReturnsOkResult()
        {
            // Act
            var result = _controller.GetProductById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var product = Assert.IsType<Product>(okResult.Value);
            Assert.Equal("Product1", product.Name);
        }

        [Fact]
        public void InsertProduct_ValidObject_ReturnsOkResult()
        {
            // Arrange
            var newProduct = new Models.AddProductsDto
            {
                Name = "Product3",
                Description = "Description3",
                Price = 30.0M,
                StockQuantity = 300
            };

            // Act
            var result = _controller.InsertProduct(newProduct);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var product = Assert.IsType<Product>(okResult.Value);
            Assert.Equal("Product3", product.Name);
        }

        [Fact]
        public void UpdateProduct_UnknownId_ReturnsNotFoundResult()
        {
            // Arrange
            var updateProduct = new Models.UpdateProductsDto
            {
                Name = "UpdatedProduct",
                Description = "UpdatedDescription",
                Price = 50.0M,
                StockQuantity = 500
            };

            // Act
            var result = _controller.UpdateProduct(3, updateProduct);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void UpdateProduct_ExistingId_ReturnsOkResult()
        {
            // Arrange
            var updateProduct = new Models.UpdateProductsDto
            {
                Name = "UpdatedProduct",
                Description = "UpdatedDescription",
                Price = 50.0M,
                StockQuantity = 500
            };

            // Act
            var result = _controller.UpdateProduct(1, updateProduct);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var product = Assert.IsType<Product>(okResult.Value);
            Assert.Equal("UpdatedProduct", product.Name);
        }

        [Fact]
        public void DeleteProduct_UnknownId_ReturnsNotFoundResult()
        {
            // Act
            var result = _controller.DeleteProduct(3);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteProduct_ExistingId_ReturnsOkResult()
        {
            // Act
            var result = _controller.DeleteProduct(1);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(1, _context.Products.Count());
        }
    }
}
