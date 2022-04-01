using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.KMAPI.Items.ItemTemplates
{
    public class ItemTemplate : Item
    {
        public ItemTemplate()
        {
            Discriminator = "ItemTemplate";
        }

        public int ExpirationDays { get; set; }
    }
}
