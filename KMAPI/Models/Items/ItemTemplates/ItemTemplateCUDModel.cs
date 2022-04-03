using KitchenManager.KMAPI.Icons;
using KitchenManager.KMAPI.ItemTags;
using System.Collections.Generic;

namespace KitchenManager.KMAPI.Items.ItemTemplates.DTO
{
    public class ItemTemplateCUDModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public int ExpirationDays { get; set; } = -1;
        public Icon Icon { get; set; }
        public List<ItemTag> ItemTags { get; set; }
    }
}
