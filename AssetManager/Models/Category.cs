using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AssetManager.Models
{
    public class Category
    {
        public Category()
        {
            Assets = new HashSet<Asset>();
            CategoryRules = new HashSet<CategoryRule>();
        }
        public int Id { get; set; }
        
        [Required]
        [Display(Name="Category Name")]
        public string Name { get; set; }

        [Required]
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<CategoryRule> CategoryRules { get; set; }

        [Display(Name="Creation Date")]
        public DateTime DateTimeCreated { get; set; }
    }
}