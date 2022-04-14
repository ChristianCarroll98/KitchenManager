using System.Collections.Generic;
using System.Linq;

namespace KitchenManager.API.ItemsNS.ItemTemplatesNS.DTO
{
    public class ItemTemplateReadDTO
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public int ExpirationDays { get; set; }
        public string IconPath { get; set; }
        public List<string> ItemTagNames { get; set; }

        public ItemTemplateReadDTO(ItemTemplateModel itemTemplate)
        {
            Name = itemTemplate.Name;
            Brand = itemTemplate.Brand;
            Description = itemTemplate.Description;
            ExpirationDays = itemTemplate.ExpirationDays;
            IconPath = itemTemplate.Icon.Path;
            ItemTagNames = itemTemplate.ItemTags.Select(it => it.Name).ToList();
        }
    }
}