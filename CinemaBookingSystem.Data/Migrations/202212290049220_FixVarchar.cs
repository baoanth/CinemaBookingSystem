namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixVarchar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Username", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 50, fixedLength: true, unicode: false));
            AlterColumn("dbo.Users", "Username", c => c.String(nullable: false, maxLength: 50, fixedLength: true, unicode: false));
        }
    }
}
