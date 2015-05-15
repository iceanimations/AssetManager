using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AssetManager.Models
{
    public class ProjectRule
    {
        [Column(Order=0), Key, ForeignKey("Project")]
        [Required]
        public int ProjectId { get; set; }
        
        [Column(Order=1), Key, ForeignKey("User")]
        [Required]
        public int UserId { get; set; }

        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
    }
}