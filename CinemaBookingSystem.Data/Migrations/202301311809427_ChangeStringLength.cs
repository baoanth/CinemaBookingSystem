namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStringLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movies", "Cast", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Movies", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "Description", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Movies", "Cast", c => c.String(nullable: false, maxLength: 256));
        }
    }
}
