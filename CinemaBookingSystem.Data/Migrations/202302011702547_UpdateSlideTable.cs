namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSlideTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Slides", "Url");
            DropColumn("dbo.Slides", "SortOrder");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Slides", "SortOrder", c => c.Int());
            AddColumn("dbo.Slides", "Url", c => c.String(maxLength: 256));
        }
    }
}
