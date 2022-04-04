using KitchenManager.KMAPI.ItemTags;

namespace KitchenManager.KMAPI.Models.ItemTags.DTO
{
    public class ItemTagDTO
    {
        public string Name { get; set; } = string.Empty;

        public ItemTagDTO(ItemTag itemTag)
        {
            Name = itemTag.Name;
        }
    }
}
