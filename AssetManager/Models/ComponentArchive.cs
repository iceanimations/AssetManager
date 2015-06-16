using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace AssetManager.Models
{
    public class ComponentArchive
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [DisplayName("Archive Date")]
        public DateTime ArchiveDate { get; set; }

        [Required]
        [ForeignKey("Component")]
        public int ComponentId { get; set; }

        [Required]
        [StringLength(255)]
        public string FilePath { get; set; }

        public virtual Component Component { get; set; }

        [DisplayName("Component Creation Date")]
        public DateTime ComponentDateTimeCreated { get; set; }
        [DisplayName("Component Modification Date")]
        public DateTime ComponentDateTimeUpdated { get; set; }
    }
}