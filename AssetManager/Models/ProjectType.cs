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
        [RegularExpression("[A-Za-z0-9]{3,50}", ErrorMessage = "Alpha-numeric allowed (3 to 50 characters)")]
        public string Type { get; set; }
        
        [Required]
        [Display(Name="UNC Location")]
        [RegularExpression("[A-Za-z0-9_\\-\\\\]{3,255}", ErrorMessage = "Alpha-numeric, underscore, back-slashes allowed (3 to 255 characters)")]
        public string LocationUNC { get; set; }

        [Required]
        [Display(Name="Display Location")]
        [RegularExpression("[A-Za-z0-9_:\\\\]{3,255}", ErrorMessage = "Alpha-numeric, underscore, colon, back-slashes allowed (3 to 255 characters)")]
        public string LocationDisplay { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

    }
}