﻿using KitchenManager.API.UserListsNS.DTO;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace KitchenManager.API.UsersNS.DTO
{
    public class UserReadDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<UserListReadMinDTO> UserLists { get; set; }
        public List<UserClaimReadDTO> UserClaims { get; set; }

        public UserReadDTO(User user = null, List<IdentityUserClaim<int>> userClaims = null)
        {
            UserName = user.UserName ?? string.Empty;
            Email = user.Email ?? string.Empty;
            PhoneNumber = user.PhoneNumber ?? string.Empty;
            UserLists = user.UserLists.Select(ul => new UserListReadMinDTO(ul)).ToList() ?? new List<UserListReadMinDTO>();
            UserClaims = userClaims.Select(uc => new UserClaimReadDTO(uc)).ToList() ?? new List<UserClaimReadDTO>();
        }
    }
}