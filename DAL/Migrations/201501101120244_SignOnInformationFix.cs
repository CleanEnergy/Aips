namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SignOnInformationFix : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.SignOnInformations", name: "Course_CourseId", newName: "CourseId");
            RenameColumn(table: "dbo.SignOnInformations", name: "SchoolYear_SchoolYearId", newName: "SchoolYearId");
            RenameColumn(table: "dbo.SignOnInformations", name: "Student_Id", newName: "StudentId");
            RenameIndex(table: "dbo.SignOnInformations", name: "IX_SchoolYear_SchoolYearId", newName: "IX_SchoolYearId");
            RenameIndex(table: "dbo.SignOnInformations", name: "IX_Student_Id", newName: "IX_StudentId");
            RenameIndex(table: "dbo.SignOnInformations", name: "IX_Course_CourseId", newName: "IX_CourseId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.SignOnInformations", name: "IX_CourseId", newName: "IX_Course_CourseId");
            RenameIndex(table: "dbo.SignOnInformations", name: "IX_StudentId", newName: "IX_Student_Id");
            RenameIndex(table: "dbo.SignOnInformations", name: "IX_SchoolYearId", newName: "IX_SchoolYear_SchoolYearId");
            RenameColumn(table: "dbo.SignOnInformations", name: "StudentId", newName: "Student_Id");
            RenameColumn(table: "dbo.SignOnInformations", name: "SchoolYearId", newName: "SchoolYear_SchoolYearId");
            RenameColumn(table: "dbo.SignOnInformations", name: "CourseId", newName: "Course_CourseId");
        }
    }
}
