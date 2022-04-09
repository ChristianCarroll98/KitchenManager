using Microsoft.AspNetCore.Identity;

namespace KitchenManager.API.UsersNS.DTO
{
    public class UserClaimReadDTO
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public UserClaimReadDTO(IdentityUserClaim<int> userClaim = null)
        {
            Type = userClaim.ClaimType ?? string.Empty;
            Value = userClaim.ClaimValue ?? string.Empty;
        }
    }
}
