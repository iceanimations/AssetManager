namespace AssetManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectTypeAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetRules",
                c => new
                    {
                        AssetId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AssetId, t.UserId })
                .ForeignKey("dbo.Assets", t => t.AssetId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.AssetId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Assets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        Thumbnail = c.String(maxLength: 255),
                        DateTimeCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        DateTimeCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.CategoryRules",
                c => new
                    {
                        CategoryId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CategoryId, t.UserId })
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComponentRules",
                c => new
                    {
                        ComponentId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ComponentId, t.UserId })
                .ForeignKey("dbo.Components", t => t.ComponentId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ComponentId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Components",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        AssetId = c.Int(nullable: false),
                        FilePath = c.String(maxLength: 255),
                        Locked = c.Boolean(nullable: false),
                        Description = c.String(),
                        DateTimeCreated = c.DateTime(nullable: false),
                        DateTimeUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.AssetId, cascadeDelete: true)
                .Index(t => t.AssetId);
            
            CreateTable(
                "dbo.ProjectRules",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.UserId })
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Thumbnail = c.String(),
                        Description = c.String(nullable: false),
                        PrefixPath = c.String(nullable: false),
                        ProjectTypeId = c.Int(nullable: false),
                        DateTimeCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectTypes", t => t.ProjectTypeId, cascadeDelete: true)
                .Index(t => t.ProjectTypeId);
            
            CreateTable(
                "dbo.ProjectTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        type = c.String(nullable: false, maxLength: 50),
                        LocationUNC = c.String(nullable: false, maxLength: 255),
                        LocationDisplay = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetRules", "UserId", "dbo.Users");
            DropForeignKey("dbo.AssetRules", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.Assets", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Categories", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.CategoryRules", "UserId", "dbo.Users");
            DropForeignKey("dbo.ProjectRules", "UserId", "dbo.Users");
            DropForeignKey("dbo.ProjectRules", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "ProjectTypeId", "dbo.ProjectTypes");
            DropForeignKey("dbo.ComponentRules", "UserId", "dbo.Users");
            DropForeignKey("dbo.ComponentRules", "ComponentId", "dbo.Components");
            DropForeignKey("dbo.Components", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.CategoryRules", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Projects", new[] { "ProjectTypeId" });
            DropIndex("dbo.ProjectRules", new[] { "UserId" });
            DropIndex("dbo.ProjectRules", new[] { "ProjectId" });
            DropIndex("dbo.Components", new[] { "AssetId" });
            DropIndex("dbo.ComponentRules", new[] { "UserId" });
            DropIndex("dbo.ComponentRules", new[] { "ComponentId" });
            DropIndex("dbo.CategoryRules", new[] { "UserId" });
            DropIndex("dbo.CategoryRules", new[] { "CategoryId" });
            DropIndex("dbo.Categories", new[] { "ProjectId" });
            DropIndex("dbo.Assets", new[] { "CategoryId" });
            DropIndex("dbo.AssetRules", new[] { "UserId" });
            DropIndex("dbo.AssetRules", new[] { "AssetId" });
            DropTable("dbo.ProjectTypes");
            DropTable("dbo.Projects");
            DropTable("dbo.ProjectRules");
            DropTable("dbo.Components");
            DropTable("dbo.ComponentRules");
            DropTable("dbo.Users");
            DropTable("dbo.CategoryRules");
            DropTable("dbo.Categories");
            DropTable("dbo.Assets");
            DropTable("dbo.AssetRules");
        }
    }
}
