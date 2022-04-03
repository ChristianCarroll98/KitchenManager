using KitchenManager.KMAPI.Shared;
using System.Collections.Generic;

namespace KitchenManager.KMAPI.Items.ItemTemplates.DTO
{
    public class ItemTemplatesResponse : ResponseBase
    {
        public List<ItemTemplate> ItemTemplates { get; set; } = new List<ItemTemplate>();
    }
}
