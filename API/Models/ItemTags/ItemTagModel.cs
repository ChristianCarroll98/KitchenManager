using KitchenManager.API.ItemsNS;
using KitchenManager.API.SharedNS.StatusNS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitchenManager.API.ItemTagsNS
{
    public class ItemTagModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Pinned { get; set; } = false;
        public List<ItemModel> Items { get; set; } = new();
        public Status Status { get; set; } = Status.inactive;
    }
}
