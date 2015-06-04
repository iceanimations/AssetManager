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
            UserList = new MultiSelectList(db.Users.ToList(), "Id", "Name");
        }

        ~CategoryViewModel()
        {
            ((IDisposable)db).Dispose();
        }

        public int Id { get; set; }
        
        [Required]
        [Display(Name="Category Name")]
        [RegularExpression("[A-Za-z0-9_/]{3,200}", ErrorMessage = "Alpha-numeric, underscore and slash (/) allowed (3 to 200 characters)")]
        public string Name { get; set; }

        [Required]
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int[] UserIds { get; set; }
        public MultiSelectList UserList { get; set; }

        [Display(Name="Creation Date")]
        public DateTime DateTimeCreated { get; set; }
    }
}