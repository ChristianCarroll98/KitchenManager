using KitchenManager.KMAPI.Shared;
using System.Collections.Generic;

namespace KitchenManager.KMAPI.ItemTags.DTO
{
    public class ItemTagsResponse : ResponseBase
    {
        public List<ItemTag> ItemTags { get; set; }
    }
}
