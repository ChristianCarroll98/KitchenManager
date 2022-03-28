using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.KMAPI
{
    public class KMRole : IdentityRole<int>
    {
        // My Customized Identity Role Class
        public KMRole(string name) : base(name)
        {
            Name = name;
        }

        //public KMRole() : base()
        //{
        //}
    }
}
