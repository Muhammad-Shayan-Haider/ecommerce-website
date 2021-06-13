namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reviewAndRating : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        userId = c.String(nullable: false, maxLength: 128),
                        ItemId = c.Int(nullable: false),
                        ratingValue = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.userId, t.ItemId })
                .ForeignKey("dbo.AspNetUsers", t => t.userId, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.userId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewId = c.Int(nullable: false, identity: true),
                        text = c.String(nullable: false),
                        reviewDate = c.DateTime(nullable: false),
                        uId = c.String(maxLength: 128),
                        itemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewId)
                .ForeignKey("dbo.AspNetUsers", t => t.uId)
                .ForeignKey("dbo.Items", t => t.itemId, cascadeDelete: true)
                .Index(t => t.uId)
                .Index(t => t.itemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "itemId", "dbo.Items");
            DropForeignKey("dbo.Reviews", "uId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ratings", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Ratings", "userId", "dbo.AspNetUsers");
            DropIndex("dbo.Reviews", new[] { "itemId" });
            DropIndex("dbo.Reviews", new[] { "uId" });
            DropIndex("dbo.Ratings", new[] { "ItemId" });
            DropIndex("dbo.Ratings", new[] { "userId" });
            DropTable("dbo.Reviews");
            DropTable("dbo.Ratings");
        }
    }
}
