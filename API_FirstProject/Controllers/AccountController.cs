using API_FirstProject.DTOs;
using API_FirstProject.Models;
using API_FirstProject.SD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace API_FirstProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(ApplicationDTO applicationDTO)
        {

            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new(SD.SD.AdminRole));
             
                await roleManager.CreateAsync(new(SD.SD.CustomerRole));
            }

            ApplicationUser user = new ApplicationUser()
            {

                UserName = applicationDTO.Name,
                Email = applicationDTO.Email,
                Address = applicationDTO.Address,

               
            };

      var result=    await  userManager.CreateAsync(user, applicationDTO.Password);

            if (result.Succeeded) {


               await userManager.AddToRoleAsync(user,SD.SD.CustomerRole);
                await signInManager.SignInAsync(user, true);
                 
                return Ok(applicationDTO);
            }
            return BadRequest(result.Errors);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(loginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                //1.chrck user name 
                var applicationuser = await userManager.FindByNameAsync(loginVM.UserName);

                if (applicationuser != null)
                {
                    //2. check password
                    var result = await userManager.CheckPasswordAsync(applicationuser, loginVM.passward);
                    if (result)
                    {
                        // 3login
                        await signInManager.SignInAsync(applicationuser, loginVM.Remembermy);
                        return Ok();
                    }
                    else
                    {
                        ModelState.AddModelError("passward", "Invalid passward");
                        return BadRequest(ModelState);
                    }
                }
                else
                {

                    ModelState.AddModelError("UserName", "Invalid user name");
                    return BadRequest(ModelState);
                }

            }

            return NotFound();
        }
        [HttpDelete("Logout")]
        public async Task<IActionResult> Logout()
        {
           await signInManager.SignOutAsync();

            return Ok();
        }

    }
}
