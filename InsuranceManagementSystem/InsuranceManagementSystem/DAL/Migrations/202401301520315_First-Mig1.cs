namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMig1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Policies", "PolicyType", c => c.Int(nullable: false));
            AddColumn("dbo.Policies", "Price", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Policies", "Price");
            DropColumn("dbo.Policies", "PolicyType");
        }
    }
}
