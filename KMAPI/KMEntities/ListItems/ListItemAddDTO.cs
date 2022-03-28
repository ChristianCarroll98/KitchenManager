using System;
using System.Collections.Generic;

namespace KitchenManager.KMAPI
{
    public class ListItemAddDTO
    {
        //should be set automatically
        public int ListId { get; set; }

        //[MaxLength(256)], throws Microsoft.EntityFrameworkCore.DbUpdateException if longer
        public string Name { get; set; }

        //[MaxLength(256)], throws Microsoft.EntityFrameworkCore.DbUpdateException if longer
        public string Description { get; set; }

        public int Quantity { get; set; }

        public DateTime ExpirationDate { get; set; }

        public List<ItemTag> ItemTags { get; set; }
    }
}
