﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_System_IntegrationTest.Utilities
{
    // a class for providing a token
    public class TestJwtToken
    {
        public List<Claim> Claims { get; } = new();
        public int ExpiresInMinutes { get; set; } = 30;

        public TestJwtToken WithRole(string roleName)
        {
            Claims.Add(new Claim(ClaimTypes.Role, roleName));
            return this;
        }

        public TestJwtToken WithUserName(string username)
        {
            Claims.Add(new Claim(ClaimTypes.Upn, username));
            return this;
        }

        public TestJwtToken WithEmail(string email)
        {
            Claims.Add(new Claim(ClaimTypes.Email, email));
            return this;
        }

        

        public TestJwtToken WithExpiration(int expiresInMinutes)
        {
            ExpiresInMinutes = expiresInMinutes;
            return this;
        }

        // generate a token
        public string Build()
        {
            var token = new JwtSecurityToken(
                JwtTokenProvider.Issuer,
                JwtTokenProvider.Issuer,
                Claims,
                expires: DateTime.Now.AddMinutes(ExpiresInMinutes),
                signingCredentials: JwtTokenProvider.SigningCredentials
            );
            return JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
