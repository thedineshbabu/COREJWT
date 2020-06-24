using System.Security.Claims;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using corejwt.Model;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace corejwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly JWTSettings _jwtSettings;
        public ValuesController(IOptions<JWTSettings> jwtsettings)
        {
            _jwtSettings = jwtsettings.Value;
        }

        [HttpPost]
        public ActionResult<User> Login([FromBody] User user) 
        {
            if (user.Name != "Dinesh" || user.Password != "123456")
            {
                return NotFound();
            }
            else 
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
                var tokenDescriptior = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]{
                        new Claim(ClaimTypes.Name, user.Name)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(2),
                    SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials( 
                    new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key), 
                    Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptior);
                user.Token = tokenHandler.WriteToken(token);
                return user;
            }
        } 
        // GET api/values
        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // // POST api/values
        // [HttpPost]
        // public void Post([FromBody] string value)
        // {
        // }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
