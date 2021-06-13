namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class img : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "imgPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "imgPath");
        }
    }
}
