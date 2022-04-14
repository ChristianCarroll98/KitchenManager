using KitchenManager.API.IconsNS;
using KitchenManager.API.ItemTagsNS;
using KitchenManager.API.SharedNS.StatusNS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenManager.API.ItemsNS
{
    public abstract class ItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; } = "None";
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; } = Status.active;

        public IconModel Icon { get; set; }
        public List<ItemTagModel> ItemTags { get; set; } = new List<ItemTagModel>();
    }
}
