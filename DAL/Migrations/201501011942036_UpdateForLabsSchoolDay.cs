namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateForLabsSchoolDay : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Labs", "SchoolDayId", c => c.Int(nullable: false));
            CreateIndex("dbo.Labs", "SchoolDayId");
            AddForeignKey("dbo.Labs", "SchoolDayId", "dbo.SchoolDays", "SchoolDayId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Labs", "SchoolDayId", "dbo.SchoolDays");
            DropIndex("dbo.Labs", new[] { "SchoolDayId" });
            DropColumn("dbo.Labs", "SchoolDayId");
        }
    }
}
