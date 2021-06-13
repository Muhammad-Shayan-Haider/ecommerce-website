namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class secondhanditems1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SecondHandItems", "location", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SecondHandItems", "location");
        }
    }
}
