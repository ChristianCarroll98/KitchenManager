using System.ComponentModel.DataAnnotations;

namespace KitchenManager.API.ItemsNS.ItemTemplatesNS
{
    public class ItemTemplateModel : ItemModel
    {
        [Range(1, int.MaxValue)]
        public int? ExpirationDays { get; set; }

        public ItemTemplateModel()
        {
            Discriminator = "ItemTemplate";
        }
    }
}
