using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sample.Common;
using Sample.DAL.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Sample.Web.Controllers
{
    public class AccountController : Controller
    {
        private List<Account> users = new List<Account>
        {
            new Account { Login = "admin@admin.com", Password = "qwerty123", Role = "admin", Age = 18, FirstName = "Aleksei", LastName = "Shukan" },
            new Account { Login = "admin2@admin.com", Password = "qwerty123", Role = "admin", Age = 18, FirstName = "Petr", LastName = "Shukan" },
            new Account { Login = "admin3@admin.com", Password = "qwerty123", Role = "admin", Age = 17, FirstName = "Aleksei", LastName = "Shukan" },
            new Account { Login = "test@test.com", Password = "qwerty123", Role = "user", Age = 17, FirstName = "Petr", LastName = "Petrov" }
        };

        [HttpPost("/token")]
        public IActionResult Token(string login, string password)
        {
            var identity = GetIdentity(login, password);

            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid login or password." });
            }

            var now = DateTime.UtcNow;

            var jwtToken = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.SecurityKey, SecurityAlgorithms.HmacSha256));

            var encodedJwtToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var response = new
            {
                access_token = encodedJwtToken,
                login = identity.Name
            };

            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string login, string password)
        {
            var user = users.FirstOrDefault(x => x.Login == login && x.Password == password);

            if (user == null)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),
                new Claim("firstName", user.FirstName),
                new Claim("age", user.Age.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Token", 
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
