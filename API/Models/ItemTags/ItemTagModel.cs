using KitchenManager.API.ItemsNS;
using KitchenManager.API.SharedNS.StatusNS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitchenManager.API.ItemTagsNS
{
    public class ItemTagModel
    {
        [Required]
        public int Id { get; set; } = 0;

        [Required]
        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        public bool Pinned { get; set; } = false;

        public List<ItemModel> Items { get; set; } = new();

        public Status Status { get; set; } = Status.inactive;
    }
}
