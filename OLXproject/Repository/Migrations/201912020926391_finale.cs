namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finale : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ratings", "ratingValue", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ratings", "ratingValue", c => c.Int(nullable: false));
        }
    }
}
