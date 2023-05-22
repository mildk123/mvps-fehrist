using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;

[assembly: OwinStartup(typeof(fehrist.Startup))]

namespace fehrist
{
    public partial class Startup
    {
      

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.UseJwtBearerAuthentication(
               new JwtBearerAuthenticationOptions
               {
                   AuthenticationMode = AuthenticationMode.Active,
                   TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = "https://www.fyp-kiet.somee.com", //some string, normally web url,  
                       ValidAudience = "https://www.fyp-kiet.somee.com",
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("kiet_Secretkey_10664noorahmed10618"))
                   }
               });
           //app.UseCors();
        }
    }
}
