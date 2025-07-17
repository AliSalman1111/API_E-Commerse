using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.ComponentModel.DataAnnotations;

namespace API_FirstProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [ValidateNever]
        public string photo { get; set; }
        [Range(0, 1000000)]
        public double price { get; set; }
        public int categoryId { get; set; }
        public int? countaty { get; set; }
        public int? companyId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
       



    }

}
