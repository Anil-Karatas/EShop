using EShop.Business.Dtos;
using EShop.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Business.Services
{
    public interface IUserService
    {
        ServiceMessage AddUser(AddUserDto addUserDto);
        UserInfoDto LoginUser(LoginUserDto loginUserDto);
        List<UserInfoDto> GetUsers();
    }
}
