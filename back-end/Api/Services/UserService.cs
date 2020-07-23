using Api.Interface;
using Api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;

        public IOptions<AppSettings> AppSettings { get; }

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            AppSettings = appSettings;
        }

       

        public UserModel Authenticate(Login login)
        {
            var auth = login.User;



            if (auth == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier.ToString(), auth.Id.ToString()),
                    new Claim(ClaimTypes.Name.ToString(), auth.Id.ToString()),
                 
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            auth.Token = tokenHandler.WriteToken(token);
            return auth;
        }

       
    }
}
