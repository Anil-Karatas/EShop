using EShop.Data.Enum;
using System.Security.Claims;

namespace EShop.WebUI.Extensions
{
    // Cookie'de tutulan verilerin Ipntrollerinin yapmak için yazaılacak metotları bu class içerisinde tutuyorum.
    public static class ClaimPrincipalExtensions
    {
        // this -> Bu sayade metot artık sondan çağrılır.
        public static bool IsLogged(this ClaimsPrincipal user)
        {
            if (user.Claims.FirstOrDefault(x => x.Type == "id") != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string GetUserFirstName(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == "firstName")?.Value;
        }
        public static string GetUserLastName(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == "lastName")?.Value;
        }
        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            var userType = user.Claims.FirstOrDefault(x => x.Type == "userType")?.Value;
            if(userType == UserTypeEnum.Admin.ToString()) {
            return true;}
            else
            {
                return false;
            }
        }
    }
}
