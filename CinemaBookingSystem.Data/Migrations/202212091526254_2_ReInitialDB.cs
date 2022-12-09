namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_ReInitialDB : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Seats", "TheatreID", "dbo.Theatres");
            DropForeignKey("dbo.TicketBookings", "BookingID", "dbo.Bookings");
            DropForeignKey("dbo.Tickets", "SeatID", "dbo.Seats");
            DropForeignKey("dbo.TicketBookings", "TicketID", "dbo.Tickets");
            DropIndex("dbo.Seats", new[] { "TheatreID" });
            DropIndex("dbo.TicketBookings", new[] { "BookingID" });
            DropIndex("dbo.TicketBookings", new[] { "TicketID" });
            DropIndex("dbo.Tickets", new[] { "SeatID" });
            CreateTable(
                "dbo.ScreeningPositions",
                c => new
                    {
                        PositionID = c.Int(nullable: false, identity: true),
                        Row = c.String(nullable: false),
                        Column = c.String(nullable: false),
                        IsBooked = c.Boolean(nullable: false),
                        ScreeningID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PositionID)
                .ForeignKey("dbo.Screenings", t => t.ScreeningID, cascadeDelete: true)
                .Index(t => t.ScreeningID);
            
            CreateTable(
                "dbo.BookingDetails",
                c => new
                    {
                        BookingID = c.Int(nullable: false),
                        PositionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BookingID, t.PositionID })
                .ForeignKey("dbo.Bookings", t => t.BookingID, cascadeDelete: true)
                .ForeignKey("dbo.ScreeningPositions", t => t.PositionID, cascadeDelete: true)
                .Index(t => t.BookingID)
                .Index(t => t.PositionID);
            
            AddColumn("dbo.Bookings", "BookedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Bookings", "IsPayed", c => c.Boolean(nullable: false));
            DropTable("dbo.Seats");
            DropTable("dbo.TicketBookings");
            DropTable("dbo.Tickets");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        TicketID = c.Int(nullable: false, identity: true),
                        TicketPrice = c.Int(nullable: false),
                        IsBooked = c.Boolean(nullable: false),
                        SeatID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TicketID);
            
            CreateTable(
                "dbo.TicketBookings",
                c => new
                    {
                        BookingID = c.Int(nullable: false),
                        TicketID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BookingID, t.TicketID });
            
            CreateTable(
                "dbo.Seats",
                c => new
                    {
                        SeatID = c.Int(nullable: false, identity: true),
                        Row = c.String(nullable: false, maxLength: 5),
                        Column = c.Int(nullable: false),
                        TheatreID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SeatID);
            
            DropForeignKey("dbo.BookingDetails", "PositionID", "dbo.ScreeningPositions");
            DropForeignKey("dbo.BookingDetails", "BookingID", "dbo.Bookings");
            DropForeignKey("dbo.ScreeningPositions", "ScreeningID", "dbo.Screenings");
            DropIndex("dbo.BookingDetails", new[] { "PositionID" });
            DropIndex("dbo.BookingDetails", new[] { "BookingID" });
            DropIndex("dbo.ScreeningPositions", new[] { "ScreeningID" });
            DropColumn("dbo.Bookings", "IsPayed");
            DropColumn("dbo.Bookings", "BookedAt");
            DropTable("dbo.BookingDetails");
            DropTable("dbo.ScreeningPositions");
            CreateIndex("dbo.Tickets", "SeatID");
            CreateIndex("dbo.TicketBookings", "TicketID");
            CreateIndex("dbo.TicketBookings", "BookingID");
            CreateIndex("dbo.Seats", "TheatreID");
            AddForeignKey("dbo.TicketBookings", "TicketID", "dbo.Tickets", "TicketID", cascadeDelete: true);
            AddForeignKey("dbo.Tickets", "SeatID", "dbo.Seats", "SeatID", cascadeDelete: true);
            AddForeignKey("dbo.TicketBookings", "BookingID", "dbo.Bookings", "BookingID", cascadeDelete: true);
            AddForeignKey("dbo.Seats", "TheatreID", "dbo.Theatres", "TheatreID", cascadeDelete: true);
        }
    }
}
