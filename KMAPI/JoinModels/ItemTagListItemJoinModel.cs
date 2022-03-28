using System.ComponentModel.DataAnnotations;

namespace KitchenManager.KMAPI
{
    public class ItemTagListItemJoinModel
    {
        [Required]
        public int ItemTagId { get; set; }

        [Required]
        public int ListItemId { get; set; }
    }
}
