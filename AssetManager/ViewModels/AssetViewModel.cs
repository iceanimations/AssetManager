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
    public class AssetViewModel
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public AssetViewModel()
        {
            UserList = new MultiSelectList(db.Users.ToList(), "Id", "Name");
        }
        public int Id { get; set; }
        [Required]
        [DisplayName("Asset Name")]
        public string Name { get; set; }
        
        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public int[] UserIds { get; set; }
        public MultiSelectList UserList { get; set; }

        [StringLength(255)]
        public string Thumbnail { get; set; }
        [Display(Name="Creation Date")]
        public DateTime DateTimeCreated { get; set; }
    }
}