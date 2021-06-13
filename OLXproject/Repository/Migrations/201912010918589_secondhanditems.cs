namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class secondhanditems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SecondHandItems",
                c => new
                    {
                        itemID = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        price = c.Double(nullable: false),
                        description = c.String(nullable: false),
                        imgPath = c.String(),
                        cId = c.Int(nullable: false),
                        uId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.itemID)
                .ForeignKey("dbo.AspNetUsers", t => t.uId)
                .ForeignKey("dbo.Categories", t => t.cId, cascadeDelete: true)
                .Index(t => t.cId)
                .Index(t => t.uId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SecondHandItems", "cId", "dbo.Categories");
            DropForeignKey("dbo.SecondHandItems", "uId", "dbo.AspNetUsers");
            DropIndex("dbo.SecondHandItems", new[] { "uId" });
            DropIndex("dbo.SecondHandItems", new[] { "cId" });
            DropTable("dbo.SecondHandItems");
        }
    }
}
