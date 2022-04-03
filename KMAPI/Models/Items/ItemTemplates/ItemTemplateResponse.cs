using KitchenManager.KMAPI.Shared;

namespace KitchenManager.KMAPI.Items.ItemTemplates.DTO
{
    public class ItemTemplateResponse : ResponseBase
    {
        public ItemTemplate ItemTemplate { get; set; } = null;
    }
}
