using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.API.Data.Seed
{
    public class ItemTemplateSeedModel
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public int ExpirationDays { get; set; }
    }
}
