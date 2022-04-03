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
    public enum ItemTemplateStatus
    {
        visible,
        invisible,
        deleted
    }

    public class ItemTemplate : Item
    {
        public ItemTemplate()
        {
            Discriminator = "ItemTemplate";
        }

        public int ExpirationDays { get; set; } = -1;

        public ItemTemplateStatus Status { get; set; } = ItemTemplateStatus.visible;

        /*public List<ItemTag> GetItemTags()
        {
            List<ItemTag> itemTags = new List<ItemTag>();
            foreach (ItemTag itemTag in this.ItemTags)
            {
                itemTags.Add(new ItemTag() { Name = itemTag.Name, Id = itemTag.Id });
            }
        }*/
    }
}
