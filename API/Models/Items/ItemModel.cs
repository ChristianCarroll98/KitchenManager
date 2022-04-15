using KitchenManager.API.IconsNS;
using KitchenManager.API.ItemTagsNS;
using KitchenManager.API.SharedNS.StatusNS;
using System;
using System.Collections.Generic;

namespace KitchenManager.API.ItemsNS
{
    public abstract class ItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; } = "None";
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; } = Status.active;
        public DateTime DeletedDate { get; set; } = DateTime.MaxValue;

        public IconModel Icon { get; set; }
        public List<ItemTagModel> ItemTags { get; set; } = new List<ItemTagModel>();
    }
}
