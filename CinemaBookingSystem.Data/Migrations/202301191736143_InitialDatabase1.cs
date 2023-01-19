namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabase1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Provinces", newName: "Locations");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Locations", newName: "Provinces");
        }
    }
}
