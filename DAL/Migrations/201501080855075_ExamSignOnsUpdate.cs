namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExamSignOnsUpdate : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.SignOnExams", name: "Exam_ExamId", newName: "ExamId");
            RenameColumn(table: "dbo.SignOnExams", name: "Student_Id", newName: "StudentId");
            RenameIndex(table: "dbo.SignOnExams", name: "IX_Student_Id", newName: "IX_StudentId");
            RenameIndex(table: "dbo.SignOnExams", name: "IX_Exam_ExamId", newName: "IX_ExamId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.SignOnExams", name: "IX_ExamId", newName: "IX_Exam_ExamId");
            RenameIndex(table: "dbo.SignOnExams", name: "IX_StudentId", newName: "IX_Student_Id");
            RenameColumn(table: "dbo.SignOnExams", name: "StudentId", newName: "Student_Id");
            RenameColumn(table: "dbo.SignOnExams", name: "ExamId", newName: "Exam_ExamId");
        }
    }
}
