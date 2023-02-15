namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVerifyStringToBooking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "VerifyCode", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "VerifyCode");
        }
    }
}
