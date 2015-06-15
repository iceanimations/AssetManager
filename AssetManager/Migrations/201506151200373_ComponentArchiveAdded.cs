namespace AssetManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComponentArchiveAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComponentArchives",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArchiveDate = c.DateTime(nullable: false),
                        ComponentId = c.Int(nullable: false),
                        FilePath = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Components", t => t.ComponentId, cascadeDelete: true)
                .Index(t => t.ComponentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComponentArchives", "ComponentId", "dbo.Components");
            DropIndex("dbo.ComponentArchives", new[] { "ComponentId" });
            DropTable("dbo.ComponentArchives");
        }
    }
}
