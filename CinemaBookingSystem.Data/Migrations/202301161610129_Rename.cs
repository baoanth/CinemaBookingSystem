namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carousels", "CarouselImage", c => c.String(maxLength: 256));
            DropColumn("dbo.Carousels", "ImageURL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Carousels", "ImageURL", c => c.String(maxLength: 256));
            DropColumn("dbo.Carousels", "CarouselImage");
        }
    }
}
