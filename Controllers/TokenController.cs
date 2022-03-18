using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiTipoCambio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _config;
        public TokenController(IConfiguration config)
        {
            _config = config;
        }


        [HttpGet]
        public async Task<JsonResult> GetGenerarToken()
        {
            string _secret = _config.GetSection("JwtConfig").GetSection("secret").Value;
            string _expDate = _config.GetSection("JwtConfig").GetSection("expirationInMinutes").Value;            
            var key = Encoding.ASCII.GetBytes(_secret);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Email, "jaleso77@mail.com"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_expDate)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);
            string bearer_token = tokenHandler.WriteToken(createdToken);

            var jsonString = new JsonResult(bearer_token);
            return jsonString;
        }


    }
}
