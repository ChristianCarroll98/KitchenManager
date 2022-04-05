using KitchenManager.API.ItemsNS;
using KitchenManager.API.UserListsNS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenManager.API.IconsNS
{
    public class Icon
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int Name { get; set; }

        [MaxLength(256)]
        [Column(TypeName = "varchar(256)")]
        public string Path { get; set; }
    }
}
