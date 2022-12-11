namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3_ReInitialDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "MovieID", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "MovieID");
            AddForeignKey("dbo.Comments", "MovieID", "dbo.Movies", "MovieID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "MovieID", "dbo.Movies");
            DropIndex("dbo.Comments", new[] { "MovieID" });
            DropColumn("dbo.Comments", "MovieID");
        }
    }
}
