using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace API_FirstProject.Models
{
    public class Category
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Name is required")]
        [MinLength(3)]
        [MaxLength(25)]
        // [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }


        


    }
}
