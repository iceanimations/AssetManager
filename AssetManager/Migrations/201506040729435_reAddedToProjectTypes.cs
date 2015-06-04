namespace AssetManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reAddedToProjectTypes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProjectTypes", "Type", c => c.String(nullable: false));
            AlterColumn("dbo.ProjectTypes", "LocationUNC", c => c.String(nullable: false));
            AlterColumn("dbo.ProjectTypes", "LocationDisplay", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectTypes", "LocationDisplay", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.ProjectTypes", "LocationUNC", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.ProjectTypes", "Type", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
