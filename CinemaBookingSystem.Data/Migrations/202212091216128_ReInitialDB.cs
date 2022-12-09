namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReInitialDB : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Screening", newName: "Screenings");
            DropForeignKey("dbo.Tickets", "ScreeningID", "dbo.Screening");
            DropIndex("dbo.Tickets", new[] { "ScreeningID" });
            DropColumn("dbo.Tickets", "ScreeningID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "ScreeningID", c => c.Int(nullable: false));
            CreateIndex("dbo.Tickets", "ScreeningID");
            AddForeignKey("dbo.Tickets", "ScreeningID", "dbo.Screening", "ScreeningID", cascadeDelete: true);
            RenameTable(name: "dbo.Screenings", newName: "Screening");
        }
    }
}
