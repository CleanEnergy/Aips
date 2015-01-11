namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExamUpdate : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Exams", name: "Subject_SubjectId", newName: "SubjectId");
            RenameIndex(table: "dbo.Exams", name: "IX_Subject_SubjectId", newName: "IX_SubjectId");
            AddColumn("dbo.Exams", "ClassroomId", c => c.Int(nullable: false));
            CreateIndex("dbo.Exams", "ClassroomId");
            AddForeignKey("dbo.Exams", "ClassroomId", "dbo.Classrooms", "ClassroomId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Exams", "ClassroomId", "dbo.Classrooms");
            DropIndex("dbo.Exams", new[] { "ClassroomId" });
            DropColumn("dbo.Exams", "ClassroomId");
            RenameIndex(table: "dbo.Exams", name: "IX_SubjectId", newName: "IX_Subject_SubjectId");
            RenameColumn(table: "dbo.Exams", name: "SubjectId", newName: "Subject_SubjectId");
        }
    }
}
