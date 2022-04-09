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
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Brand { get; set; } = "None";

        [MaxLength(256)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(256)]
        [Column(TypeName = "varchar(256)")]
        public string Discriminator { get; set; }

        public IconModel Icon { get; set; }

        public List<ItemTagModel> ItemTags { get; set; } = new List<ItemTagModel>();

        public Status Status { get; set; } = Status.active;
    }
}
