namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubjectsForCourses : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SchoolYearUsers", "SchoolYear_SchoolYearId", "dbo.SchoolYears");
            DropForeignKey("dbo.SchoolYearUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubjectStudent", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubjectStudent", "SubjectId", "dbo.Subjects");
            DropIndex("dbo.SubjectStudent", new[] { "SubjectId" });
            DropIndex("dbo.SubjectStudent", new[] { "StudentId" });
            DropIndex("dbo.SchoolYearUsers", new[] { "SchoolYear_SchoolYearId" });
            DropIndex("dbo.SchoolYearUsers", new[] { "User_Id" });
            AddColumn("dbo.AspNetUsers", "SchoolYear_SchoolYearId", c => c.Int());
            AddColumn("dbo.Subjects", "Course_CourseId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "SchoolYear_SchoolYearId");
            CreateIndex("dbo.Subjects", "Course_CourseId");
            AddForeignKey("dbo.AspNetUsers", "SchoolYear_SchoolYearId", "dbo.SchoolYears", "SchoolYearId");
            AddForeignKey("dbo.Subjects", "Course_CourseId", "dbo.Courses", "CourseId");
            DropTable("dbo.SubjectStudent");
            DropTable("dbo.SchoolYearUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SchoolYearUsers",
                c => new
                    {
                        SchoolYear_SchoolYearId = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SchoolYear_SchoolYearId, t.User_Id });
            
            CreateTable(
                "dbo.SubjectStudent",
                c => new
                    {
                        SubjectStudentId = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        StudentId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.SubjectStudentId);
            
            DropForeignKey("dbo.Subjects", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.AspNetUsers", "SchoolYear_SchoolYearId", "dbo.SchoolYears");
            DropIndex("dbo.Subjects", new[] { "Course_CourseId" });
            DropIndex("dbo.AspNetUsers", new[] { "SchoolYear_SchoolYearId" });
            DropColumn("dbo.Subjects", "Course_CourseId");
            DropColumn("dbo.AspNetUsers", "SchoolYear_SchoolYearId");
            CreateIndex("dbo.SchoolYearUsers", "User_Id");
            CreateIndex("dbo.SchoolYearUsers", "SchoolYear_SchoolYearId");
            CreateIndex("dbo.SubjectStudent", "StudentId");
            CreateIndex("dbo.SubjectStudent", "SubjectId");
            AddForeignKey("dbo.SubjectStudent", "SubjectId", "dbo.Subjects", "SubjectId", cascadeDelete: true);
            AddForeignKey("dbo.SubjectStudent", "StudentId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SchoolYearUsers", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SchoolYearUsers", "SchoolYear_SchoolYearId", "dbo.SchoolYears", "SchoolYearId", cascadeDelete: true);
        }
    }
}
