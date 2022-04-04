using KitchenManager.KMAPI.Icons;
using KitchenManager.KMAPI.ItemTags;
using KitchenManager.KMAPI.Shared;
using System.Collections.Generic;
using System.Linq;

namespace KitchenManager.KMAPI.Items.ItemTemplates.DTO
{
    public class ItemTemplateListResponse : ResponseBase
    {
        public List<ItemTemplateDTO> ITResponseDTOs { get; set; }

        public void SetITResponseDTOsFromITList(List<ItemTemplate> itemTemplates)
        {
            ITResponseDTOs = itemTemplates.Select(it => new ItemTemplateDTO(it)).ToList();
        }
    }
}
