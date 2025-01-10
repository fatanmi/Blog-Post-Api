using Blog_Post_Api.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary.Implementation.Contract;
using ServerLibrary.Model.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServerLibrary.Implementation.Repositories
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApiUser> _UserManager;
        private readonly IConfiguration _Configuration;
        private ApiUser _User;
        private readonly IConfigurationSection _JWTSettings;

        public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _UserManager = userManager;
            _Configuration = configuration;
            _JWTSettings = _Configuration.GetSection("JwtSetting");
        }

        public async Task<string> CreateToken()
        {
            SigningCredentials SignCredential = GetSigninCredentials();
            IEnumerable<Claim> Claims = await GetClaims();
            JwtSecurityToken TokenOptionsClaim = GenerateTokeOptions(SignCredential, Claims);

            return new JwtSecurityTokenHandler().WriteToken(TokenOptionsClaim);
        }

        private JwtSecurityToken GenerateTokeOptions(SigningCredentials SignCredentials, IEnumerable<Claim> claims)
        {
            JwtSecurityToken Token = new(issuer: _JWTSettings["Issuer"],
                                         audience: _JWTSettings["Audience"],
                                         claims: claims,
                                         expires: DateTime.UtcNow.AddSeconds(Convert.ToDouble(_JWTSettings["Lifetime"])),
                                         signingCredentials: SignCredentials);

            return Token;
        }

        private async Task<IEnumerable<Claim>> GetClaims()
        {
            var Claims = new List<Claim>()
            {
                new (ClaimTypes.Name, _User.UserName),
                new (ClaimTypes.Email, _User.Email)
            };

            IEnumerable<string> Roles = await _UserManager.GetRolesAsync(_User);
            foreach (string Role in Roles)
            {
                Claims.Add(new(ClaimTypes.Role, Role));
            }

            return Claims;
        }

        private static SigningCredentials GetSigninCredentials()
        {
            string Key = Environment.GetEnvironmentVariable("Key");
            SymmetricSecurityKey Secrete = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            return new SigningCredentials(Secrete, SecurityAlgorithms.HmacSha256);
        }

        public async Task<bool> ValidateUser(LoginUserDTO UserDTO)
        {
            _User = await _UserManager.FindByEmailAsync(UserDTO.Email);

            return _User != null
                && await _UserManager.CheckPasswordAsync(_User, UserDTO.Password);
        }
    }
}
