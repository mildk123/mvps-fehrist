using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace fehrist.Helper
{
    public class token_handler
    {
        public virtual Object GetUserToken(int? roleID, string roleName, int accountID, string name, string email, string status)
        {
            string key = "kiet_Secretkey_10664noorahmed10618"; //Secret key which will be used later during validation    
            var issuer = "https://www.fyp-kiet.somee.com";  //normally this will be your site URL    

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("roleID", roleID.ToString()));
            permClaims.Add(new Claim("roleName", roleName));
            permClaims.Add(new Claim("accountID", accountID.ToString()));
            permClaims.Add(new Claim("name", name));
            permClaims.Add(new Claim("email", email));
            permClaims.Add(new Claim("status", status));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddHours(6),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return new { data = jwt_token };
        }
    }
}