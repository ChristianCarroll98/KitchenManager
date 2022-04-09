using System.Collections.Generic;
using System.Linq;

namespace KitchenManager.API.ItemsNS.ItemTemplatesNS.DTO
{
    public class ItemTemplateReadDTO
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public int? ExpirationDays { get; set; }
        public string IconPath { get; set; }
        public List<string> ItemTagNames { get; set; }

        public ItemTemplateReadDTO(ItemTemplateModel itemTemplate = null)
        {
            Name = itemTemplate.Name ?? string.Empty;
            Brand = itemTemplate.Brand ?? string.Empty;
            Description = itemTemplate.Description ?? string.Empty;
            ExpirationDays = itemTemplate == null ? -1 : itemTemplate.ExpirationDays;
            IconPath = itemTemplate.Icon.Path ?? string.Empty;
            ItemTagNames = itemTemplate.ItemTags.Select(it => it.Name).ToList() ?? new List<string>();
        }
    }
}