using Blog_Post_Api.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ServerLibrary.Data;
using ServerLibrary.Model.Entities;
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

        public static void ConfigureErrorHandler(this IApplicationBuilder App)
        {

            App.UseExceptionHandler(error =>
            {
                error.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var exception = contextFeature.Error;

                    // Log detailed error information
                    Log.Error($"An error occurred: {exception.Message}");
                    Log.Error($"Stack Trace: {exception.StackTrace}");

                    // Include stack trace in response (optional, for development purposes only)
                    var ErrorDetails = new Error
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal Server Error.",
                        Details = exception.Message,
                        StackTrace = exception.StackTrace
                    };

                    await context.Response.WriteAsync(ErrorDetails.ToString());


                };
            }
            );
            });
        }
    }
}
