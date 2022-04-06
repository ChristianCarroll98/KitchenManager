using KitchenManager.API.IconsNS.DTO;
using KitchenManager.API.ItemTagsNS.DTO;
using System.Collections.Generic;
using System.Linq;

namespace KitchenManager.API.ItemsNS.ItemTemplatesNS.DTO
{
    public class ItemTemplateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ExpirationDays { get; set; } = -1;
        public IconDTO IconDTO { get; set; } = new();
        public List<ItemTagDTO> ItemTagDTOs { get; set; } = new List<ItemTagDTO>();

        public ItemTemplateDTO(){}
        public ItemTemplateDTO(ItemTemplate itemTemplate)
        {
            Name = itemTemplate.Name;
            Brand = itemTemplate.Brand;
            Description = itemTemplate.Description;
            ExpirationDays = itemTemplate.ExpirationDays;
            IconDTO = new IconDTO(itemTemplate.Icon);
            ItemTagDTOs = itemTemplate.ItemTags.Select(it => new ItemTagDTO(it)).ToList();
        }
    }
}