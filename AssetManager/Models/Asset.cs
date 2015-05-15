using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AssetManager.Models
{
    public class Asset
    {
        public Asset()
        {
            Components = new HashSet<Component>();
            AssetRules = new HashSet<AssetRule>();
        }
        public int Id { get; set; }
        [Required]
        [DisplayName("Asset Name")]
        public string Name { get; set; }
        
        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Component> Components { get; set; }
        public virtual ICollection<AssetRule> AssetRules { get; set; }

        [StringLength(255)]
        public string Thumbnail { get; set; }
        [Display(Name="Creation Date")]
        public DateTime DateTimeCreated { get; set; }
    }
}