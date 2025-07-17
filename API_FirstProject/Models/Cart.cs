using System.ComponentModel.DataAnnotations.Schema;

namespace API_FirstProject.Models
{
    public class Cart
    {

        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }

        public int count { get; set; }

    }
}
