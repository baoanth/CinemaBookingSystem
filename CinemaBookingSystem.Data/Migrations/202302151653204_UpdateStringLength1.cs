namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStringLength1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Articles", "ArticleTitle", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Users", "FullName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Users", "Address", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Payments", "PaymentMethod", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Cinemas", "FAX", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Comments", "Content", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Movies", "Director", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "Director", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Comments", "Content", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Cinemas", "FAX", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Payments", "PaymentMethod", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Users", "Address", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Users", "FullName", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.Articles", "ArticleTitle", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
