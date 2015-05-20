using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AssetManager.Models;
using System.Web.Mvc;

namespace AssetManager.ViewModels
{
    public class CategoryViewModel
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public CategoryViewModel()
        {
            //Assets = new HashSet<Asset>();
            //CategoryRules = new HashSet<CategoryRule>();
        }
        public int Id { get; set; }
        
        [Required]
        [Display(Name="Category Name")]
        public string Name { get; set; }

        [Required]
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        public int[] UserIds { get; set; }
        public MultiSelectList UserList { get; set; }

        //public virtual Project Project { get; set; }
        //public virtual ICollection<Asset> Assets { get; set; }
        //public virtual ICollection<CategoryRule> CategoryRules { get; set; }

        [Display(Name="Creation Date")]
        public DateTime DateTimeCreated { get; set; }
    }
}