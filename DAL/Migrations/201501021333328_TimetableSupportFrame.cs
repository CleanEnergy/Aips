namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimetableSupportFrame : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classrooms",
                c => new
                    {
                        ClassroomId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 10),
                        ClassroomTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClassroomId)
                .ForeignKey("dbo.ClassroomTypes", t => t.ClassroomTypeId, cascadeDelete: true)
                .Index(t => t.ClassroomTypeId);
            
            CreateTable(
                "dbo.ClassroomTypes",
                c => new
                    {
                        ClassroomTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.ClassroomTypeId);
            
            CreateTable(
                "dbo.ScheduledClassrooms",
                c => new
                    {
                        ClassroomId = c.Int(nullable: false),
                        ScheduleStart = c.DateTime(nullable: false),
                        ScheduleEnd = c.DateTime(nullable: false),
                        SchoolDayId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClassroomId)
                .ForeignKey("dbo.Classrooms", t => t.ClassroomId)
                .ForeignKey("dbo.SchoolDays", t => t.SchoolDayId, cascadeDelete: true)
                .Index(t => t.ClassroomId)
                .Index(t => t.SchoolDayId);
            
            AddColumn("dbo.Labs", "ClassroomId", c => c.Int(nullable: false));
            AddColumn("dbo.Lectures", "ScheduledClassroomId", c => c.Int(nullable: false));
            AddColumn("dbo.Lectures", "SchoolDayId", c => c.Int(nullable: false));
            AddColumn("dbo.Lectures", "ScheduledClassroom_ClassroomId", c => c.Int());
            CreateIndex("dbo.Labs", "ClassroomId");
            CreateIndex("dbo.Lectures", "SchoolDayId");
            CreateIndex("dbo.Lectures", "ScheduledClassroom_ClassroomId");
            AddForeignKey("dbo.Labs", "ClassroomId", "dbo.Classrooms", "ClassroomId", cascadeDelete: true);
            AddForeignKey("dbo.Lectures", "ScheduledClassroom_ClassroomId", "dbo.ScheduledClassrooms", "ClassroomId");
            AddForeignKey("dbo.Lectures", "SchoolDayId", "dbo.SchoolDays", "SchoolDayId", cascadeDelete: true);
            DropColumn("dbo.Lectures", "StartTime");
            DropColumn("dbo.Lectures", "EndTime");
            DropColumn("dbo.Lectures", "Day");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Lectures", "Day", c => c.Int(nullable: false));
            AddColumn("dbo.Lectures", "EndTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Lectures", "StartTime", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Lectures", "SchoolDayId", "dbo.SchoolDays");
            DropForeignKey("dbo.Lectures", "ScheduledClassroom_ClassroomId", "dbo.ScheduledClassrooms");
            DropForeignKey("dbo.ScheduledClassrooms", "SchoolDayId", "dbo.SchoolDays");
            DropForeignKey("dbo.ScheduledClassrooms", "ClassroomId", "dbo.Classrooms");
            DropForeignKey("dbo.Labs", "ClassroomId", "dbo.Classrooms");
            DropForeignKey("dbo.Classrooms", "ClassroomTypeId", "dbo.ClassroomTypes");
            DropIndex("dbo.ScheduledClassrooms", new[] { "SchoolDayId" });
            DropIndex("dbo.ScheduledClassrooms", new[] { "ClassroomId" });
            DropIndex("dbo.Lectures", new[] { "ScheduledClassroom_ClassroomId" });
            DropIndex("dbo.Lectures", new[] { "SchoolDayId" });
            DropIndex("dbo.Labs", new[] { "ClassroomId" });
            DropIndex("dbo.Classrooms", new[] { "ClassroomTypeId" });
            DropColumn("dbo.Lectures", "ScheduledClassroom_ClassroomId");
            DropColumn("dbo.Lectures", "SchoolDayId");
            DropColumn("dbo.Lectures", "ScheduledClassroomId");
            DropColumn("dbo.Labs", "ClassroomId");
            DropTable("dbo.ScheduledClassrooms");
            DropTable("dbo.ClassroomTypes");
            DropTable("dbo.Classrooms");
        }
    }
}
