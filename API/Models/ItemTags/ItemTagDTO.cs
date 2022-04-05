namespace KitchenManager.API.ItemTagsNS.DTO
{
    public class ItemTagDTO
    {
        public string Name { get; set; } = string.Empty;

        public ItemTagDTO(){}
        public ItemTagDTO(ItemTag itemTag)
        {
            Name = itemTag.Name;
        }
    }
}
