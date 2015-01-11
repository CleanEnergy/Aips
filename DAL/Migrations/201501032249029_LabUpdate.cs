namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LabUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Labs", "ClassroomId", "dbo.Classrooms");
            DropForeignKey("dbo.Labs", "SchoolDayId", "dbo.SchoolDays");
            DropIndex("dbo.Labs", new[] { "SchoolDayId" });
            DropIndex("dbo.Labs", new[] { "ClassroomId" });
            AddColumn("dbo.Labs", "ScheduledClassroomId", c => c.Int(nullable: false));
            CreateIndex("dbo.Labs", "ScheduledClassroomId");
            AddForeignKey("dbo.Labs", "ScheduledClassroomId", "dbo.ScheduledClassrooms", "ScheduledClassroomId");
            DropColumn("dbo.Labs", "StartTime");
            DropColumn("dbo.Labs", "EndTime");
            DropColumn("dbo.Labs", "MinimalPresencePercent");
            DropColumn("dbo.Labs", "SchoolDayId");
            DropColumn("dbo.Labs", "ClassroomId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Labs", "ClassroomId", c => c.Int(nullable: false));
            AddColumn("dbo.Labs", "SchoolDayId", c => c.Int(nullable: false));
            AddColumn("dbo.Labs", "MinimalPresencePercent", c => c.Int(nullable: false));
            AddColumn("dbo.Labs", "EndTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Labs", "StartTime", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Labs", "ScheduledClassroomId", "dbo.ScheduledClassrooms");
            DropIndex("dbo.Labs", new[] { "ScheduledClassroomId" });
            DropColumn("dbo.Labs", "ScheduledClassroomId");
            CreateIndex("dbo.Labs", "ClassroomId");
            CreateIndex("dbo.Labs", "SchoolDayId");
            AddForeignKey("dbo.Labs", "SchoolDayId", "dbo.SchoolDays", "SchoolDayId");
            AddForeignKey("dbo.Labs", "ClassroomId", "dbo.Classrooms", "ClassroomId");
        }
    }
}
