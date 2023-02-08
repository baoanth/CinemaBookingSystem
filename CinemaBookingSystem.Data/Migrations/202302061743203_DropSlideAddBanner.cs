namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropSlideAddBanner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "BannerImg", c => c.String(maxLength: 256));
            DropTable("dbo.Slides");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Slides",
                c => new
                    {
                        SlideId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 256),
                        Image = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.SlideId);
            
            DropColumn("dbo.Movies", "BannerImg");
        }
    }
}
