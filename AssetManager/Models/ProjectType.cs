using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AssetManager.Models
{
    public class ProjectType
    {
        public ProjectType()
        {
            Projects = new HashSet<Project>();
        }

        public int Id { get; set; }
        
        [Required]
        [Display(Name="Project Type")]
        [StringLength(50)]
        public string Type { get; set; }
        
        [Required]
        [StringLength(255)]
        [Display(Name="UNC Location")]
        public string LocationUNC { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name="Display Location")]
        public string LocationDisplay { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

    }
}