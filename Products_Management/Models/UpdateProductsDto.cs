using System.ComponentModel.DataAnnotations;

namespace Products_Management.Models
{
    public class UpdateProductsDto
    {
        //public required string Name { get; set; }
        //public required string Description { get; set; }
        //public required int Price { get; set; }
        //public required int StockQuantity { get; set; }
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(500)]
        public required string Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }
    }
}
