namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLectureLab : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ScheduledClassrooms", "SubjectId", "dbo.Subjects");
            DropIndex("dbo.ScheduledClassrooms", new[] { "SubjectId" });
            AddColumn("dbo.Labs", "AssistantId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Lectures", "ProfessorId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Labs", "AssistantId");
            CreateIndex("dbo.Lectures", "ProfessorId");
            AddForeignKey("dbo.Labs", "AssistantId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Lectures", "ProfessorId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.ScheduledClassrooms", "SubjectId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ScheduledClassrooms", "SubjectId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Lectures", "ProfessorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Labs", "AssistantId", "dbo.AspNetUsers");
            DropIndex("dbo.Lectures", new[] { "ProfessorId" });
            DropIndex("dbo.Labs", new[] { "AssistantId" });
            DropColumn("dbo.Lectures", "ProfessorId");
            DropColumn("dbo.Labs", "AssistantId");
            CreateIndex("dbo.ScheduledClassrooms", "SubjectId");
            AddForeignKey("dbo.ScheduledClassrooms", "SubjectId", "dbo.Subjects", "SubjectId");
        }
    }
}
