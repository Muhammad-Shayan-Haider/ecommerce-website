namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WishList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WishLists",
                c => new
                    {
                        userId = c.String(nullable: false, maxLength: 128),
                        ItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.userId, t.ItemId })
                .ForeignKey("dbo.AspNetUsers", t => t.userId, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.userId)
                .Index(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WishLists", "ItemId", "dbo.Items");
            DropForeignKey("dbo.WishLists", "userId", "dbo.AspNetUsers");
            DropIndex("dbo.WishLists", new[] { "ItemId" });
            DropIndex("dbo.WishLists", new[] { "userId" });
            DropTable("dbo.WishLists");
        }
    }
}
