using KitchenManager.API.IconsNS.DTO;
using System.Collections.Generic;

namespace KitchenManager.API.ItemsNS.ItemTemplatesNS.DTO
{
    public class ItemTemplateCreateUpdateDTO
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public int? ExpirationDays { get; set; }
        public IconCreateUpdateDTO IconCreateUpdateDTO { get; set; }
        public List<string> ItemTagNames { get; set; }
    }
}