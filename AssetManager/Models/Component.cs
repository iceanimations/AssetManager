using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetManager.Models
{
    public class Component
    {
        public Component()
        {
            ComponentRules = new HashSet<ComponentRule>();
        }
        public int Id { get; set; }
        [Required]
        [DisplayName("Component Name")]
        public string Name { get; set; }
        
        [Required]
        [ForeignKey("Asset")]
        public int AssetId { get; set; }

        public virtual Asset Asset { get; set; }
        public virtual ICollection<ComponentRule> ComponentRules { get; set; }

        [StringLength(255)]
        public string FilePath { get; set; }
        public bool Locked { get; set; }
        public string Description { get; set; }

        [Required]
        [Display(Name="Creation Date")]
        public DateTime DateTimeCreated { get; set; }
        [Required]
        [Display(Name="Modification Date")]
        public DateTime DateTimeUpdated { get; set; }
    }
}