namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class condition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SecondHandItems", "ReasonForSelling", c => c.String());
            AddColumn("dbo.SecondHandItems", "Condition", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SecondHandItems", "Condition");
            DropColumn("dbo.SecondHandItems", "ReasonForSelling");
        }
    }
}
