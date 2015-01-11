namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LecturesKeyFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lectures", "ScheduledClassroomId", c => c.Int(nullable: false));
            CreateIndex("dbo.Lectures", "ScheduledClassroomId");
            AddForeignKey("dbo.Lectures", "ScheduledClassroomId", "dbo.ScheduledClassrooms", "ScheduledClassroomId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lectures", "ScheduledClassroomId", "dbo.ScheduledClassrooms");
            DropIndex("dbo.Lectures", new[] { "ScheduledClassroomId" });
            DropColumn("dbo.Lectures", "ScheduledClassroomId");
        }
    }
}
