using KitchenManager.KMAPI.Shared;

namespace KitchenManager.KMAPI.Items.ItemTemplates.DTO
{
    public class ItemTemplateResponse : ResponseBase
    {
        public ItemTemplateDTO ITResponseDTO { get; set; } = new ItemTemplateDTO();
    }
}
