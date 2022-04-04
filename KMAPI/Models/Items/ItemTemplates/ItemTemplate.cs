using KitchenManager.KMAPI.Items.ItemTemplates.DTO;
using KitchenManager.KMAPI.ItemTags;
using KitchenManager.KMAPI.Shared;
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
        public int ExpirationDays { get; set; } = -1;

        public ItemTemplate()
        {
            Discriminator = "ItemTemplate";
        }
    }
}
