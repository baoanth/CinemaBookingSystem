namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateScreeningAttribute2nd : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Screenings", "IsFull");
            DropColumn("dbo.Screenings", "EmptySeats");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Screenings", "EmptySeats", c => c.Int(nullable: false));
            AddColumn("dbo.Screenings", "IsFull", c => c.Boolean(nullable: false));
        }
    }
}
