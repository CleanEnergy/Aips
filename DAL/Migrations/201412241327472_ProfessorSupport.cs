namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfessorSupport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactHours",
                c => new
                    {
                        ContactHoursId = c.Int(nullable: false, identity: true),
                        Cabinet = c.String(nullable: false, maxLength: 10),
                        Day = c.String(nullable: false, maxLength: 20),
                        Time = c.String(nullable: false, maxLength: 4),
                        Professor_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ContactHoursId)
                .ForeignKey("dbo.AspNetUsers", t => t.Professor_Id, cascadeDelete: true)
                .Index(t => t.Professor_Id);
            
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        ExamId = c.Int(nullable: false, identity: true),
                        DateAndTime = c.DateTime(nullable: false),
                        Subject_SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExamId)
                .ForeignKey("dbo.Subjects", t => t.Subject_SubjectId, cascadeDelete: true)
                .Index(t => t.Subject_SubjectId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        SubjectId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SubjectId);
            
            CreateTable(
                "dbo.ExamStudent",
                c => new
                    {
                        ExamStudentId = c.Int(nullable: false, identity: true),
                        Grade = c.Int(nullable: false),
                        ExamId = c.Int(nullable: false),
                        StudentId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ExamStudentId)
                .ForeignKey("dbo.Exams", t => t.ExamId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.ExamId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.SignOnExams",
                c => new
                    {
                        SignOnExamId = c.Int(nullable: false, identity: true),
                        Exam_ExamId = c.Int(nullable: false),
                        Student_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.SignOnExamId)
                .ForeignKey("dbo.Exams", t => t.Exam_ExamId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Student_Id, cascadeDelete: true)
                .Index(t => t.Exam_ExamId)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.SignOnInformations",
                c => new
                    {
                        SignOnInformationId = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Faculty = c.String(nullable: false, maxLength: 50),
                        Course = c.String(nullable: false, maxLength: 50),
                        Degree = c.String(nullable: false, maxLength: 5),
                        Student_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.SignOnInformationId)
                .ForeignKey("dbo.AspNetUsers", t => t.Student_Id, cascadeDelete: true)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.SubjectStudent",
                c => new
                    {
                        SubjectStudentId = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        StudentId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.SubjectStudentId)
                .ForeignKey("dbo.AspNetUsers", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.Theses",
                c => new
                    {
                        ThesisId = c.Int(nullable: false, identity: true),
                        FileBytes = c.Binary(nullable: false),
                        Author_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ThesisId)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id, cascadeDelete: true)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.UniversityEvents",
                c => new
                    {
                        UniversityEventId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        DateAndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UniversityEventId);
            
            AddColumn("dbo.AspNetUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Theses", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubjectStudent", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.SubjectStudent", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SignOnInformations", "Student_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SignOnExams", "Student_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SignOnExams", "Exam_ExamId", "dbo.Exams");
            DropForeignKey("dbo.ExamStudent", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ExamStudent", "ExamId", "dbo.Exams");
            DropForeignKey("dbo.Exams", "Subject_SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.ContactHours", "Professor_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Theses", new[] { "Author_Id" });
            DropIndex("dbo.SubjectStudent", new[] { "StudentId" });
            DropIndex("dbo.SubjectStudent", new[] { "SubjectId" });
            DropIndex("dbo.SignOnInformations", new[] { "Student_Id" });
            DropIndex("dbo.SignOnExams", new[] { "Student_Id" });
            DropIndex("dbo.SignOnExams", new[] { "Exam_ExamId" });
            DropIndex("dbo.ExamStudent", new[] { "StudentId" });
            DropIndex("dbo.ExamStudent", new[] { "ExamId" });
            DropIndex("dbo.Exams", new[] { "Subject_SubjectId" });
            DropIndex("dbo.ContactHours", new[] { "Professor_Id" });
            DropColumn("dbo.AspNetUsers", "Discriminator");
            DropTable("dbo.UniversityEvents");
            DropTable("dbo.Theses");
            DropTable("dbo.SubjectStudent");
            DropTable("dbo.SignOnInformations");
            DropTable("dbo.SignOnExams");
            DropTable("dbo.ExamStudent");
            DropTable("dbo.Subjects");
            DropTable("dbo.Exams");
            DropTable("dbo.ContactHours");
        }
    }
}
