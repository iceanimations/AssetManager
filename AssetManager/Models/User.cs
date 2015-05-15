using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManager.Models
{
    public class User
    {
        public User()
        {
            ProjectRules = new HashSet<ProjectRule>();
            CategoryRules = new HashSet<CategoryRule>();
            AssetRules = new HashSet<AssetRule>();
            ComponentRules = new HashSet<ComponentRule>();
        }
        public int Id { get; set; }
        [Required]
        [Display(Name="Username")]
        public string Name { get; set; }

        public virtual ICollection<ProjectRule> ProjectRules { get; set; }
        public virtual ICollection<CategoryRule> CategoryRules { get; set; }
        public virtual ICollection<AssetRule> AssetRules { get; set; }
        public virtual ICollection<ComponentRule> ComponentRules { get; set; }

    }
}