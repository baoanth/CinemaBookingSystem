namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateContactDepartmentLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contacts", "Department", c => c.String(nullable: false, maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contacts", "Department", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
