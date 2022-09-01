using Business.Abstract;
using Core.Security.Hashing;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthManager : IAuthManager
    {
        private readonly IUserManager _userManager;

        public AuthManager(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public string Login(LoginDTO loginDTO)
        {
            var checkUser = _userManager.GetByEmail(loginDTO.Email);
            if(checkUser == null)
                 return null;

            var checkPassword = HashingHelper.VerifyPassword(loginDTO.Password, checkUser.PasswordHash, checkUser.PasswordSalt);
            if (!checkPassword)
                return null;

            return null;
        }

        public void Register(RegisterDTO registerDTO)
        {
            throw new NotImplementedException();
        }
    }
}
