namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondMig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppliedPolicies", "PolicyType", c => c.Int(nullable: false));
            AddColumn("dbo.AppliedPolicies", "Price", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppliedPolicies", "Price");
            DropColumn("dbo.AppliedPolicies", "PolicyType");
        }
    }
}
