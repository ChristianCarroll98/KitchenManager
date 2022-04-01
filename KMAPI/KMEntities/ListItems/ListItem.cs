using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.KMAPI
{
    public class ListItem
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ListId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Brand { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public DateTime ExpirationDate { get; set; }

        [MaxLength(256)]
        [Column(TypeName = "varchar(256)")]
        public string IconPath { get; set; }

        public List<ItemTag> ItemTags { get; set; }
    }
}
