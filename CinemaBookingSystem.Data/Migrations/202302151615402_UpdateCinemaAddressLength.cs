namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCinemaAddressLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cinemas", "Address", c => c.String(nullable: false, maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cinemas", "Address", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
