using Microsoft.AspNetCore.Identity;

namespace API_FirstProject.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string Address { get; set; }

    }
}
