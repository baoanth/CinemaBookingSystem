namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingID = c.Int(nullable: false, identity: true),
                        PaymentID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingID)
                .ForeignKey("dbo.Payments", t => t.PaymentID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.PaymentID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        PaymentID = c.Int(nullable: false, identity: true),
                        PaymentMethod = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.PaymentID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50, fixedLength: true, unicode: false),
                        Password = c.String(nullable: false, maxLength: 50, fixedLength: true, unicode: false),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        EmailAddress = c.String(nullable: false, maxLength: 100),
                        PhoneNumber = c.String(nullable: false, maxLength: 10),
                        Address = c.String(nullable: false, maxLength: 500),
                        RoleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Roles", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 100),
                        RoleDescription = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.Carousels",
                c => new
                    {
                        CarouselID = c.Int(nullable: false, identity: true),
                        CarouselName = c.String(nullable: false, maxLength: 256),
                        CarouselDescription = c.String(maxLength: 256),
                        ImageURL = c.String(maxLength: 256),
                        DisplayOrder = c.Int(),
                    })
                .PrimaryKey(t => t.CarouselID);
            
            CreateTable(
                "dbo.Cinemas",
                c => new
                    {
                        CinemaID = c.Int(nullable: false, identity: true),
                        CinemaName = c.String(nullable: false, maxLength: 256),
                        Location = c.String(nullable: false, maxLength: 256),
                        FAX = c.String(nullable: false, maxLength: 50),
                        Hotline = c.String(nullable: false, maxLength: 50),
                        ProvinceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CinemaID)
                .ForeignKey("dbo.Provinces", t => t.ProvinceID, cascadeDelete: true)
                .Index(t => t.ProvinceID);
            
            CreateTable(
                "dbo.Provinces",
                c => new
                    {
                        ProvinceID = c.Int(nullable: false, identity: true),
                        ProvinceName = c.String(nullable: false, maxLength: 100),
                        Region = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ProvinceID);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false, maxLength: 500),
                        CommentedAt = c.DateTime(nullable: false),
                        CommentedBy = c.Int(nullable: false),
                        StarRated = c.Int(),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Users", t => t.CommentedBy, cascadeDelete: true)
                .Index(t => t.CommentedBy);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        MovieID = c.Int(nullable: false, identity: true),
                        MovieName = c.String(nullable: false, maxLength: 256),
                        Director = c.String(nullable: false, maxLength: 50),
                        Cast = c.String(nullable: false, maxLength: 256),
                        Genres = c.String(nullable: false, maxLength: 256),
                        RunningTime = c.Int(nullable: false),
                        Rated = c.String(maxLength: 256),
                        TrailerURL = c.String(maxLength: 256),
                        ThumpnailImg = c.String(maxLength: 256),
                        Description = c.String(nullable: false, maxLength: 1000),
                    })
                .PrimaryKey(t => t.MovieID);
            
            CreateTable(
                "dbo.Screening",
                c => new
                    {
                        ScreeningID = c.Int(nullable: false, identity: true),
                        ShowTime = c.DateTime(nullable: false),
                        ShowStatus = c.Boolean(nullable: false),
                        IsFull = c.Boolean(nullable: false),
                        EmptySeats = c.Int(nullable: false),
                        TheatreID = c.Int(nullable: false),
                        MovieID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ScreeningID)
                .ForeignKey("dbo.Movies", t => t.MovieID, cascadeDelete: true)
                .ForeignKey("dbo.Theatres", t => t.TheatreID, cascadeDelete: true)
                .Index(t => t.TheatreID)
                .Index(t => t.MovieID);
            
            CreateTable(
                "dbo.Theatres",
                c => new
                    {
                        TheatreID = c.Int(nullable: false, identity: true),
                        TheatreName = c.String(nullable: false, maxLength: 256),
                        Capacity = c.Int(nullable: false),
                        CinemaID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TheatreID)
                .ForeignKey("dbo.Cinemas", t => t.CinemaID, cascadeDelete: true)
                .Index(t => t.CinemaID);
            
            CreateTable(
                "dbo.Seats",
                c => new
                    {
                        SeatID = c.Int(nullable: false, identity: true),
                        Row = c.String(nullable: false, maxLength: 5),
                        Column = c.Int(nullable: false),
                        TheatreID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SeatID)
                .ForeignKey("dbo.Theatres", t => t.TheatreID, cascadeDelete: true)
                .Index(t => t.TheatreID);
            
            CreateTable(
                "dbo.SupportOnlines",
                c => new
                    {
                        SupportID = c.Int(nullable: false, identity: true),
                        SupportName = c.String(nullable: false, maxLength: 50),
                        Department = c.String(nullable: false, maxLength: 50),
                        Mobile = c.String(maxLength: 50),
                        FacebookURL = c.String(maxLength: 50),
                        SupportEmail = c.String(maxLength: 50),
                        IsWorking = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SupportID);
            
            CreateTable(
                "dbo.TicketBookings",
                c => new
                    {
                        BookingID = c.Int(nullable: false),
                        TicketID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BookingID, t.TicketID })
                .ForeignKey("dbo.Bookings", t => t.BookingID, cascadeDelete: true)
                .ForeignKey("dbo.Tickets", t => t.TicketID, cascadeDelete: true)
                .Index(t => t.BookingID)
                .Index(t => t.TicketID);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        TicketID = c.Int(nullable: false, identity: true),
                        TicketPrice = c.Int(nullable: false),
                        IsBooked = c.Boolean(nullable: false),
                        SeatID = c.Int(nullable: false),
                        ScreeningID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TicketID)
                .ForeignKey("dbo.Screening", t => t.ScreeningID, cascadeDelete: true)
                .ForeignKey("dbo.Seats", t => t.SeatID, cascadeDelete: true)
                .Index(t => t.SeatID)
                .Index(t => t.ScreeningID);
            
            CreateTable(
                "dbo.VisitorStatistics",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        VisitedDate = c.DateTime(nullable: false),
                        IPAddress = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketBookings", "TicketID", "dbo.Tickets");
            DropForeignKey("dbo.Tickets", "SeatID", "dbo.Seats");
            DropForeignKey("dbo.Tickets", "ScreeningID", "dbo.Screening");
            DropForeignKey("dbo.TicketBookings", "BookingID", "dbo.Bookings");
            DropForeignKey("dbo.Seats", "TheatreID", "dbo.Theatres");
            DropForeignKey("dbo.Screening", "TheatreID", "dbo.Theatres");
            DropForeignKey("dbo.Theatres", "CinemaID", "dbo.Cinemas");
            DropForeignKey("dbo.Screening", "MovieID", "dbo.Movies");
            DropForeignKey("dbo.Comments", "CommentedBy", "dbo.Users");
            DropForeignKey("dbo.Cinemas", "ProvinceID", "dbo.Provinces");
            DropForeignKey("dbo.Bookings", "UserID", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.Bookings", "PaymentID", "dbo.Payments");
            DropIndex("dbo.Tickets", new[] { "ScreeningID" });
            DropIndex("dbo.Tickets", new[] { "SeatID" });
            DropIndex("dbo.TicketBookings", new[] { "TicketID" });
            DropIndex("dbo.TicketBookings", new[] { "BookingID" });
            DropIndex("dbo.Seats", new[] { "TheatreID" });
            DropIndex("dbo.Theatres", new[] { "CinemaID" });
            DropIndex("dbo.Screening", new[] { "MovieID" });
            DropIndex("dbo.Screening", new[] { "TheatreID" });
            DropIndex("dbo.Comments", new[] { "CommentedBy" });
            DropIndex("dbo.Cinemas", new[] { "ProvinceID" });
            DropIndex("dbo.Users", new[] { "RoleID" });
            DropIndex("dbo.Bookings", new[] { "UserID" });
            DropIndex("dbo.Bookings", new[] { "PaymentID" });
            DropTable("dbo.VisitorStatistics");
            DropTable("dbo.Tickets");
            DropTable("dbo.TicketBookings");
            DropTable("dbo.SupportOnlines");
            DropTable("dbo.Seats");
            DropTable("dbo.Theatres");
            DropTable("dbo.Screening");
            DropTable("dbo.Movies");
            DropTable("dbo.Comments");
            DropTable("dbo.Provinces");
            DropTable("dbo.Cinemas");
            DropTable("dbo.Carousels");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Payments");
            DropTable("dbo.Bookings");
        }
    }
}
