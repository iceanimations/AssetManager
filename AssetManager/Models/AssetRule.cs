using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AssetManager.Models
{
    public class AssetRule
    {
        [Column(Order = 0), Key, ForeignKey("Asset")]
        [Required]
        public int AssetId { get; set; }

        [Column(Order = 1), Key, ForeignKey("User")]
        [Required]
        public int UserId { get; set; }

        public virtual Asset Asset { get; set; }
        public virtual User User { get; set; }
    }
}