using KitchenManager.KMAPI.Icons;
using KitchenManager.KMAPI.Items.ItemTemplates;
using KitchenManager.KMAPI.ItemTags;
using KitchenManager.KMAPI.Models.ItemTags.DTO;
using System.Collections.Generic;
using System.Linq;

namespace KitchenManager.KMAPI.Items.ItemTemplates.DTO
{
    public class ItemTemplateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ExpirationDays { get; set; } = -1;
        public Icon Icon { get; set; } = new Icon();
        public List<ItemTagDTO> ItemTagDTOs { get; set; } = new List<ItemTagDTO>();

        public ItemTemplateDTO(){}
        public ItemTemplateDTO(ItemTemplate itemTemplate)
        {
            setValuesFromItemTemplate(itemTemplate);
        }

        public void setValuesFromItemTemplate(ItemTemplate itemTemplate)
        {
            Name = itemTemplate.Name;
            Brand = itemTemplate.Brand;
            Description = itemTemplate.Description;
            ExpirationDays = itemTemplate.ExpirationDays;
            Icon = itemTemplate.Icon;
            ItemTagDTOs = itemTemplate.ItemTags.Select(it => new ItemTagDTO(it)).ToList();
        }
    }
}
