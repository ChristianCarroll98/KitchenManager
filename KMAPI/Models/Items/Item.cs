﻿using KitchenManager.KMAPI.Icons;
using KitchenManager.KMAPI.ItemTags;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.KMAPI.Items
{
    public enum ItemStatus
    {
        active,
        inactive,
        deleted
    }

    public abstract class Item
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

        public Icon Icon { get; set; }

        public List<ItemTag> ItemTags { get; set; } = new List<ItemTag>();

        public ItemStatus Status { get; set; } = ItemStatus.active;
    }
}
