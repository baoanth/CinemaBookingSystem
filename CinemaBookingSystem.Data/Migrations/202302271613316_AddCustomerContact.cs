namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerContact : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerContacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false, maxLength: 500),
                        CustomerEmail = c.String(nullable: false, maxLength: 500),
                        CustomerPhone = c.String(nullable: false, maxLength: 500),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CustomerContacts");
        }
    }
}
