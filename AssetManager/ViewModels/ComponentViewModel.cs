using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AssetManager.Models;
using System.Web.Mvc;

namespace AssetManager.ViewModels
{
    public class ComponentViewModel
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ComponentViewModel()
        {
            UserList = new MultiSelectList(db.Users.ToList(), "Id", "Name")
        }
        public int Id { get; set; }
        [Required]
        [DisplayName("Component Name")]
        public string Name { get; set; }

        [Required]
        [ForeignKey("Asset")]
        public int AssetId { get; set; }

        [StringLength(255)]
        public string FilePath { get; set; }
        public bool Locked { get; set; }
        public string Description { get; set; }

        public int[] UserIds { get; set; }
        public MultiSelectList UserList { get; set; }

        [Required]
        [Display(Name="Creation Date")]
        public DateTime DateTimeCreated { get; set; }
        [Required]
        [Display(Name="Modification Date")]
        public DateTime DateTimeUpdated { get; set; }
    }
}