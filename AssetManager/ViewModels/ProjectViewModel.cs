using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AssetManager.Models;
using System.Web.Mvc;
using AssetManager.Validators;

namespace AssetManager.ViewModels
{
    public class ProjectViewModel
    {
        ApplicationDbContext db = new ApplicationDbContext();
     
        public ProjectViewModel()
        {
            UserList = new MultiSelectList(db.Users.ToList(), "Id", "Name");
        }

        ~ProjectViewModel()
        {
            ((IDisposable)db).Dispose();
        }

        public int Id { get; set; }
        
        [Required]
        [Display(Name="Project Name")]
        [RegularExpression("[A-Za-z0-9_]{3,50}", ErrorMessage = "Alpha-numeric, underscore allowed (3 to 50 characters)")]
        public string Name { get; set; }

        [ValidateThumb(ErrorMessage="Image format: png, jpeg. Size: 10mb max. Resolution: square (200x200 min)")]
        public HttpPostedFileBase Thumbnail { get; set; }

        [Required]
        [StringLength(100)]
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