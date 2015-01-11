namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateForLabs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Labs", "StartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Labs", "EndTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Labs", "ScheduledOn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Labs", "ScheduledOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.Labs", "EndTime");
            DropColumn("dbo.Labs", "StartTime");
        }
    }
}
