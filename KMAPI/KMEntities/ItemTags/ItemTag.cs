using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.KMAPI
{
    public class ItemTag
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        //public List<ItemTagItemTemplateJoinModel> ItemTemplates { get; set; }
        public List<ItemTemplate> ItemTemplates { get; set; }
        //public List<ItemTagListItemJoinModel> ListItems { get; set; }
        public List<ListItem> ListItems { get; set; }
    }
}
