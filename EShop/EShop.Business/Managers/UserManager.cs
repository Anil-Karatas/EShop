using EShop.Business.Dtos;
using EShop.Business.Services;
using EShop.Business.Types;
using EShop.Data.Entities;
using EShop.Data.Enum;
using EShop.Data.Repository;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Business.Managers
{
    public class UserManager : IUserService
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IDataProtector _dataProtector;

        public UserManager(IRepository<UserEntity> userRepository, IDataProtectionProvider dataProtectionProvider )
        {
            _userRepository = userRepository;
            _dataProtector = dataProtectionProvider.CreateProtector("security");
        }
        public ServiceMessage AddUser(AddUserDto addUserDto)
        {
            //Email adresi kullanıcı adımız olacak. Bu yüzden sistemde varmı diye kontrol etmemiz gerekecek.

            var hasMail = _userRepository.GetAll(x => x.EMail.ToLower() == addUserDto.EMail.ToLower()).ToList();

            //hasmail'in içeri dolumu yoksa nullmu kontrol edecegiz.
            if (hasMail.Any())
            {
                //eğer kullanıcı adı veri tabanından çekildeyse uyarı göndereceğiz.
                return new ServiceMessage
                {
                    IsSucceed = true,
                    Message = "Bu EPosta adresli bir kullanıcı zaten mevcut."
                };
            }
            var userEntity = new UserEntity()
            {
                FistName = addUserDto.FirstName,
                LastName = addUserDto.LastName,
                EMail = addUserDto.EMail,
                Password = _dataProtector.Protect(addUserDto.Password), //şifreyi kriptolamak için yaptık.
                UserType = UserTypeEnum.User
            };
            _userRepository.Add(userEntity);
            return new ServiceMessage() { IsSucceed = true, };
        }

        public List<UserInfoDto> GetUsers()
        {
            var userInfoDto = _userRepository.GetAll().Select(x => new UserInfoDto()
            {
                Id = x.Id,
                FirstName = x.FistName,
                LastName = x.LastName,
                EMail = x.EMail,
                UserType = x.UserType,

            }).ToList();
            return userInfoDto;
        }

        public UserInfoDto LoginUser(LoginUserDto loginUserDto)
        {
            var userEntity = _userRepository.Get(x => x.EMail == loginUserDto.Email);
            if (userEntity == null)
            {
                return null;
                //form üzerinde gönderilen mail adresi ile eşleşen bir veri tabloda yoksa oturum açılamıyacak bundan dolayı geriye birşey döndürmeyecek
            }
            var rawPassword = _dataProtector.Unprotect(userEntity.Password);
            if (loginUserDto.Password == rawPassword)
            {
                return new UserInfoDto()
                {
                    Id = userEntity.Id,
                    FirstName = userEntity.FistName,
                    LastName = userEntity.LastName,
                    UserType = userEntity.UserType,
                    EMail = userEntity.EMail,
                };
            }
            else
            {
                return null;
            }
        }

    }
}
