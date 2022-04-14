using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace KitchenManager.API.UsersNS.DTO
{
    public class UserReadNoListsDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<UserClaimReadDTO> UserClaims { get; set; }

        public UserReadNoListsDTO(UserModel user = null, List<IdentityUserClaim<int>> userClaims = null)
        {
            UserName = user.UserName ?? string.Empty;
            Email = user.Email ?? string.Empty;
            PhoneNumber = user.PhoneNumber ?? string.Empty;
            UserClaims = userClaims.Select(uc => new UserClaimReadDTO(uc)).ToList() ?? new List<UserClaimReadDTO>();
        }
    }
}