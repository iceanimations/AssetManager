namespace AssetManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class compDatesAddedToArchive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComponentArchives", "ComponentDateTimeCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.ComponentArchives", "ComponentDateTimeUpdated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ComponentArchives", "ComponentDateTimeUpdated");
            DropColumn("dbo.ComponentArchives", "ComponentDateTimeCreated");
        }
    }
}
