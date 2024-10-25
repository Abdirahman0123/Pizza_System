using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pizza_System.Dtos;
using Pizza_System.Model;
using Pizza_System.Services;
using System.Net;

using System.Net.Http;
//using System.Web.Http;

namespace Pizza_System.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("v1/api/users")]
    public class UserController : ControllerBase
    {
        public IAuthenticationService authenticationService;
        public UserManager<User> userManager;


        public UserController(IAuthenticationService authenticationService, UserManager<User> userManager)
        {
            this.authenticationService = authenticationService;
            this.userManager = userManager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] RegisterRequest request)
        {
            // check if email already in use
            /*  var emailExists = userManager.FindByEmailAsync(request.Email);


            if (emailExists != null)
            {
                return Conflict("Email Already in use");
            }*/

            var userByEmail = await userManager.FindByEmailAsync(request.Email);
            //var userByUsername = await userManager.FindByNameAsync(userByEmail.Username);

            if (userByEmail is not null )
            {
                return Conflict("Email Already in use");
            }

            /*if (!ModelState.IsValid)
            {
                // Do something with the product (not shown).

                //return new HttpResponseMessage(HttpStatusCode.OK);
                return BadRequest(ModelState);
            }*/
            

            IdentityResult result = await authenticationService.Register(request);

            /*if (result.Succeeded)
            {
                return Ok("Registration Successful");
            }*/

            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginRequest request)
        {
            var user =  await userManager.FindByEmailAsync(request.Email);

            if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            {
                return NotFound("Incorrect email or password");
            }
            var result = await authenticationService.Login(request);

            return Ok(result);
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("me")]
        public async Task<ActionResult> CurrentUser()
        {
            var userFromHttpContext = await userManager.FindByNameAsync(User.Identity.Name);
            var currentUser = await userManager.FindByIdAsync(userFromHttpContext.Id);
            var user = new User
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Email = currentUser.Email,
            };
            return Ok(user);
        }
    }

}
