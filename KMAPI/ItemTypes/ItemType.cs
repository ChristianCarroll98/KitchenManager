using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.KMAPI.ItemTypes
{
    public class ItemType
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        [MaxLength(256)]
        [Column(TypeName = "varchar(256)")]
        public string IconPath { get; set; }
    }
}
