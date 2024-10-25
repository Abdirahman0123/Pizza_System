using Microsoft.AspNetCore.Identity;
using Pizza_System.Dtos;
using Pizza_System.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace Pizza_System.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public AuthenticationService(UserManager<User> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        // register a new user
        public async Task<IdentityResult> Register(RegisterRequest request)
        {
            //var userEmail = userManager.FindByEmailAsync(request.Email);

            User user = new ()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email    ,
                SecurityStamp = Guid.NewGuid().ToString()

            };

            // create a user and and give them role User
            var result = await userManager.CreateAsync(user, request.Password);

            // give default Customer role to new users
              var finalResult = await userManager.AddToRoleAsync(user, Role.Customer);
            return finalResult;
        }
        public async Task<string> Login(LoginRequest request)
        {
             // move this to controller 
            var user =   await userManager.FindByEmailAsync(request.Email);

            /*if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            {
                
            }*/
            //

            // create user claims
            var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };


            // get the user`s roles and add to the claims
            var userRoles = await userManager.GetRolesAsync(user);

            if (userRoles is not null && userRoles.Any())
            {
                authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
            }

            /* var token = GetToken(authClaims);

             return new JwtSecurityTokenHandler().WriteToken(token);*/

            JwtSecurityToken token = GetToken(authClaims);

            return new JwtSecurityTokenHandler().WriteToken(token);

            //return token;

            //throw new NotImplementedException();
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;


        }
    }
}
