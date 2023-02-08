namespace CinemaBookingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArticleModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        ArticleId = c.Int(nullable: false, identity: true),
                        ArticleTitle = c.String(nullable: false, maxLength: 50),
                        ArticleImage = c.String(maxLength: 500),
                        ArticleVideo = c.String(maxLength: 500),
                        ArticleContent = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ArticleId)
                .ForeignKey("dbo.Users", t => t.CreatedBy, cascadeDelete: true)
                .Index(t => t.CreatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles", "CreatedBy", "dbo.Users");
            DropIndex("dbo.Articles", new[] { "CreatedBy" });
            DropTable("dbo.Articles");
        }
    }
}
