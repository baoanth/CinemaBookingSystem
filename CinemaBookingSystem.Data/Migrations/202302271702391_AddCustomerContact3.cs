namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerContact3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerContacts", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerContacts", "Status");
        }
    }
}
