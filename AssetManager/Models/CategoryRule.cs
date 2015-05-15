using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AssetManager.Models
{
    public class CategoryRule
    {
        [Column(Order = 0), Key, ForeignKey("Category")]
        [Required]
        public int CategoryId { get; set; }

        [Column(Order = 1), Key, ForeignKey("User")]
        [Required]
        public int UserId { get; set; }

        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
    }
}