﻿using System;
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

        public DbSet<ProjectRule> ProjectRules { get; set; }

        public DbSet<CategoryRule> CategoryRules { get; set; }

        public DbSet<AssetRule> AssetRules { get; set; }

        public DbSet<ComponentRule> ComponentRules { get; set; }

        public System.Data.Entity.DbSet<AssetManager.Models.ProjectType> ProjectTypes { get; set; }
        

    }
}