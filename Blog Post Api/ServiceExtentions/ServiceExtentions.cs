using Blog_Post_Api.Authentication;
using Microsoft.AspNetCore.Identity;
using ServerLibrary.Data;

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
    }
}
