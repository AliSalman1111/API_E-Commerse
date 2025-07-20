using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace API_FirstProject.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [ValidateNever]

        public IFormFile photo { get; set; }
        [Range(0, 1000000)]
        public double price { get; set; }
        public int categoryId { get; set; }
        public int? countaty { get; set; }
        public int? companyId { get; set; }
    }
}
