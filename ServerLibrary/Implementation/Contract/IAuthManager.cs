using ServerLibrary.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Implementation.Contract
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUserDTO UserDTO);
        Task<string> CreateToken();

    }
}
