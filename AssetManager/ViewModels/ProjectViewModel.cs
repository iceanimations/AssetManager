using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AssetManager.Models;
using System.Web.Mvc;

namespace AssetManager.ViewModels
{
    public class ProjectViewModel
    {
        ApplicationDbContext db = new ApplicationDbContext();
     
        public ProjectViewModel()
        {
            UserList = new MultiSelectList(db.Users.ToList(), "Id", "Name");
        }
        public int Id { get; set; }
        
        [Required]
        [Display(Name="Project Name")]
        public string Name { get; set; }

        public HttpPostedFileBase Thumbnail { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [ForeignKey("ProjectType")]
        public int ProjectTypeId { get; set; }

        public int[] UserIds { get; set; }
        public MultiSelectList UserList { get; set; }

        [Display(Name="Creation Date")]
        [ScaffoldColumn(false)]
        public DateTime DateTimeCreated { get; set; }
    }
}