using Microsoft.AspNetCore.Identity;

namespace Blog_Post_Api.Authentication
{
    public class ApiUser: IdentityUser
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
    }
}
