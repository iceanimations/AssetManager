using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace AssetManager.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DbConnection") { }

        //User
        public DbSet<User> Users { get; set; }
        
        //Assets
        public DbSet<Project> Projects { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Component> Components { get; set; }

        // Rules
        public DbSet<ProjectRule> ProjectRules { get; set; }
        public DbSet<CategoryRule> CategoryRules { get; set; }
        public DbSet<AssetRule> AssetRules { get; set; }
        public DbSet<ComponentRule> ComponentRules { get; set; }

        //misc
        public DbSet<ProjectType> ProjectTypes { get; set; }
        public DbSet<ComponentArchive> ComponentArchives { get; set; }
        

    }
}