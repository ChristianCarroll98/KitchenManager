using System;
using System.Collections.Generic;
using KitchenManager.API.SharedNS.StatusNS;
using KitchenManager.API.UserListsNS;
using Microsoft.AspNetCore.Identity;

namespace KitchenManager.API.UsersNS
{
    public class UserModel : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public Status Status { get; set; } = Status.active; //inactive if.... when? or only use deleted again, so account is recoverable for some time?
        public List<UserListModel> UserLists { get; set; } = new();

        public UserModel() : base()
        {
            
            NormalizedUserName = NormalizedEmail;
            
            //disabling confirming email/phone number, lockout, and 2fa for now
            EmailConfirmed = true;
            PhoneNumberConfirmed = true;
            TwoFactorEnabled = false;
            LockoutEnabled = false;
        }
        /*
         * Create:
         * 
         * Name
         * Email
         * Password
         * PhoneNumber
         * Birthday
         * 
         * later:
         * TwoFactorEnabled
         * LockoutEnabled
         * 
         * 
         * Update:
         * 
         * Name
         * PhoneNumber
         * Birthday
         * 
         * later:
         * TwoFactorEnabled
         * LockoutEnabled
         * 
         * 
         * base properties I need to care about?:
         * 
         * public virtual string Email { get; set; }
         * public virtual bool EmailConfirmed { get; set; }
         * 
         * public virtual string PhoneNumber { get; set; }
         * public virtual bool PhoneNumberConfirmed { get; set; }
         * 
         * public virtual bool TwoFactorEnabled { get; set; }
         * 
         * public virtual int AccessFailedCount { get; set; }
         * public virtual bool LockoutEnabled { get; set; }
         * public virtual DateTimeOffset? LockoutEnd { get; set; }
         */
    }
}
