namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateScreeningAttribute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Screenings", "ShowStatus", c => c.Boolean(nullable: false));
            DropColumn("dbo.Screenings", "IsShowed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Screenings", "IsShowed", c => c.Boolean(nullable: false));
            DropColumn("dbo.Screenings", "ShowStatus");
        }
    }
}
