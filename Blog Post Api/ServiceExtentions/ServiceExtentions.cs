using Blog_Post_Api.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary.Data;
using System.Text;

namespace Blog_Post_Api.ServiceExtentions
{
    public static class ServiceExtentions
    {
        public static void ConfigureIdentity(this IServiceCollection Services)
        {
            IdentityBuilder Builder = Services.AddIdentityCore<ApiUser>(q => q.User.RequireUniqueEmail = true);
            Builder = new IdentityBuilder(Builder.UserType, typeof(IdentityRole), Services);
            Builder.AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders();

        }

        public static void ConfigureJWT(this IServiceCollection Services, IConfiguration Configuration)
        {
            IConfigurationSection JWTSettings = Configuration.GetSection("JwtSetting");
            string Key = Environment.GetEnvironmentVariable("Key");

            Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = JWTSettings["Issuer"],
                        ValidAudience = JWTSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)),
                    };
                });
            ;
        }
    }
}
