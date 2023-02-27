namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerContact2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerContacts", "SendedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerContacts", "SendedAt");
        }
    }
}
