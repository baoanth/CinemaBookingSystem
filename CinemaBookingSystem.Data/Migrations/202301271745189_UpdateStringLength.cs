namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStringLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contacts", "FacebookURL", c => c.String(maxLength: 500));
            AlterColumn("dbo.Contacts", "Message", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contacts", "Message", c => c.String(maxLength: 200));
            AlterColumn("dbo.Contacts", "FacebookURL", c => c.String(maxLength: 50));
        }
    }
}
