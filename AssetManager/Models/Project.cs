using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AssetManager.Models
{
    public class Project
    {
        public Project()
        {
            ProjectRules = new HashSet<ProjectRule>();
            Categories = new HashSet<Category>();
        }
        public int Id { get; set; }
        
        [Required]
        [Display(Name="Project Name")]
        public string Name { get; set; }

        [StringLength(255)]
        public string Thumbnail { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [ForeignKey("ProjectType")]
        public int ProjectTypeId { get; set; }

        public virtual ICollection<ProjectRule> ProjectRules { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ProjectType ProjectType { get; set; }
        

        [Display(Name="Creation Date")]
        [ScaffoldColumn(false)]
        public DateTime DateTimeCreated { get; set; }
    }
}