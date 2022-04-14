using System.ComponentModel.DataAnnotations;

namespace KitchenManager.API.ItemsNS.ItemTemplatesNS
{
    public class ItemTemplateModel : ItemModel
    {
        public int ExpirationDays { get; set; } // if <= 0, no expiration date.
    }
}
