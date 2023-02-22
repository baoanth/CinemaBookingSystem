namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        PaymentId = c.Int(nullable: false),
                        BookedAt = c.DateTime(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.Payments", t => t.PaymentId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.PaymentId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        PaymentMethod = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.PaymentId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50, unicode: false),
                        Password = c.String(nullable: false, maxLength: 50, unicode: false),
                        FullName = c.String(nullable: false, maxLength: 80),
                        DOB = c.DateTime(nullable: false),
                        Email = c.String(nullable: false, maxLength: 100),
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
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 100),
                        RoleDescription = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Cinemas",
                c => new
                    {
                        CinemaId = c.Int(nullable: false, identity: true),
                        CinemaName = c.String(nullable: false, maxLength: 256),
                        FAX = c.String(nullable: false, maxLength: 50),
                        Hotline = c.String(nullable: false, maxLength: 50),
                        Address = c.String(nullable: false, maxLength: 100),
                        City = c.String(nullable: false, maxLength: 100),
                        Region = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.CinemaId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false, maxLength: 500),
                        CommentedAt = c.DateTime(nullable: false),
                        CommentedBy = c.Int(nullable: false),
                        MovieId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Users", t => t.CommentedBy, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.MovieId, cascadeDelete: true)
                .Index(t => t.CommentedBy)
                .Index(t => t.MovieId);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        MovieId = c.Int(nullable: false, identity: true),
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
                .PrimaryKey(t => t.MovieId);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        ContactId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Department = c.String(nullable: false, maxLength: 50),
                        Mobile = c.String(maxLength: 50),
                        FacebookURL = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        Message = c.String(maxLength: 200),
                        IsWorking = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ContactId);
            
            CreateTable(
                "dbo.Errors",
                c => new
                    {
                        ErrorId = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        StackTrace = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ErrorId);
            
            CreateTable(
                "dbo.ScreeningPositions",
                c => new
                    {
                        PositionId = c.Int(nullable: false, identity: true),
                        Row = c.String(nullable: false),
                        Column = c.String(nullable: false),
                        Price = c.Int(nullable: false),
                        IsBooked = c.Boolean(nullable: false),
                        ScreeningId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PositionId)
                .ForeignKey("dbo.Screenings", t => t.ScreeningId, cascadeDelete: true)
                .Index(t => t.ScreeningId);
            
            CreateTable(
                "dbo.Screenings",
                c => new
                    {
                        ScreeningId = c.Int(nullable: false, identity: true),
                        ShowTime = c.DateTime(nullable: false),
                        IsShowed = c.Boolean(nullable: false),
                        IsFull = c.Boolean(nullable: false),
                        EmptySeats = c.Int(nullable: false),
                        TheatreId = c.Int(nullable: false),
                        MovieId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ScreeningId)
                .ForeignKey("dbo.Movies", t => t.MovieId, cascadeDelete: true)
                .ForeignKey("dbo.Theatres", t => t.TheatreId, cascadeDelete: true)
                .Index(t => t.TheatreId)
                .Index(t => t.MovieId);
            
            CreateTable(
                "dbo.Theatres",
                c => new
                    {
                        TheatreId = c.Int(nullable: false, identity: true),
                        TheatreName = c.String(nullable: false, maxLength: 256),
                        Capacity = c.Int(nullable: false),
                        CinemaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TheatreId)
                .ForeignKey("dbo.Cinemas", t => t.CinemaId, cascadeDelete: true)
                .Index(t => t.CinemaId);
            
            CreateTable(
                "dbo.Slides",
                c => new
                    {
                        SlideId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 256),
                        Url = c.String(maxLength: 256),
                        Image = c.String(maxLength: 256),
                        SortOrder = c.Int(),
                    })
                .PrimaryKey(t => t.SlideId);
            
            CreateTable(
                "dbo.BookingDetails",
                c => new
                    {
                        BookingId = c.Int(nullable: false),
                        PositionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BookingId, t.PositionId })
                .ForeignKey("dbo.Bookings", t => t.BookingId, cascadeDelete: true)
                .ForeignKey("dbo.ScreeningPositions", t => t.PositionId, cascadeDelete: true)
                .Index(t => t.BookingId)
                .Index(t => t.PositionId);
            
            CreateTable(
                "dbo.VisitorStatistics",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        VisitedDate = c.DateTime(nullable: false),
                        IPAddress = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookingDetails", "PositionId", "dbo.ScreeningPositions");
            DropForeignKey("dbo.BookingDetails", "BookingId", "dbo.Bookings");
            DropForeignKey("dbo.ScreeningPositions", "ScreeningId", "dbo.Screenings");
            DropForeignKey("dbo.Screenings", "TheatreId", "dbo.Theatres");
            DropForeignKey("dbo.Theatres", "CinemaId", "dbo.Cinemas");
            DropForeignKey("dbo.Screenings", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.Comments", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.Comments", "CommentedBy", "dbo.Users");
            DropForeignKey("dbo.Bookings", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.Bookings", "PaymentId", "dbo.Payments");
            DropIndex("dbo.BookingDetails", new[] { "PositionId" });
            DropIndex("dbo.BookingDetails", new[] { "BookingId" });
            DropIndex("dbo.Theatres", new[] { "CinemaId" });
            DropIndex("dbo.Screenings", new[] { "MovieId" });
            DropIndex("dbo.Screenings", new[] { "TheatreId" });
            DropIndex("dbo.ScreeningPositions", new[] { "ScreeningId" });
            DropIndex("dbo.Comments", new[] { "MovieId" });
            DropIndex("dbo.Comments", new[] { "CommentedBy" });
            DropIndex("dbo.Users", new[] { "RoleID" });
            DropIndex("dbo.Bookings", new[] { "UserId" });
            DropIndex("dbo.Bookings", new[] { "PaymentId" });
            DropTable("dbo.VisitorStatistics");
            DropTable("dbo.BookingDetails");
            DropTable("dbo.Slides");
            DropTable("dbo.Theatres");
            DropTable("dbo.Screenings");
            DropTable("dbo.ScreeningPositions");
            DropTable("dbo.Errors");
            DropTable("dbo.Contacts");
            DropTable("dbo.Movies");
            DropTable("dbo.Comments");
            DropTable("dbo.Cinemas");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Payments");
            DropTable("dbo.Bookings");
        }
    }
}
