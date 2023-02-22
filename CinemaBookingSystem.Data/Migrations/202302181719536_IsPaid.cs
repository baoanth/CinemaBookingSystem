namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsPaid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "IsPaid", c => c.Boolean(nullable: false));
            DropColumn("dbo.Bookings", "IsPayed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "IsPayed", c => c.Boolean(nullable: false));
            DropColumn("dbo.Bookings", "IsPaid");
        }
    }
}
