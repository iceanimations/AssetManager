namespace AssetManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectLocationRemoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Projects", "PrefixPath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "PrefixPath", c => c.String(nullable: false));
        }
    }
}
