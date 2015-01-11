namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScheduleClassroomAddSubject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScheduledClassrooms", "SubjectId", c => c.Int(nullable: false));
            CreateIndex("dbo.ScheduledClassrooms", "SubjectId");
            AddForeignKey("dbo.ScheduledClassrooms", "SubjectId", "dbo.Subjects", "SubjectId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScheduledClassrooms", "SubjectId", "dbo.Subjects");
            DropIndex("dbo.ScheduledClassrooms", new[] { "SubjectId" });
            DropColumn("dbo.ScheduledClassrooms", "SubjectId");
        }
    }
}
