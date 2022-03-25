using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.KMAPI.KMUser
{
    public class KMUser : IdentityUser<int>
    {
        // My Customized Identity User Class
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
