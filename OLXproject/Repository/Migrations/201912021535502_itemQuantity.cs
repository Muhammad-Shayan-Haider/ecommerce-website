namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class itemQuantity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "quantity");
        }
    }
}
