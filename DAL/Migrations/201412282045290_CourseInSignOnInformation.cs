namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseInSignOnInformation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "SchoolYear_SchoolYearId", "dbo.SchoolYears");
            DropForeignKey("dbo.SignOnInformations", "Faculty_FacultyId", "dbo.Faculties");
            DropIndex("dbo.Courses", new[] { "SchoolYear_SchoolYearId" });
            DropIndex("dbo.SignOnInformations", new[] { "Faculty_FacultyId" });
            AddColumn("dbo.SignOnInformations", "Course_CourseId", c => c.Int(nullable: false));
            CreateIndex("dbo.SignOnInformations", "Course_CourseId");
            AddForeignKey("dbo.SignOnInformations", "Course_CourseId", "dbo.Courses", "CourseId", cascadeDelete: true);
            DropColumn("dbo.Courses", "SchoolYear_SchoolYearId");
            DropColumn("dbo.SignOnInformations", "Faculty_FacultyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SignOnInformations", "Faculty_FacultyId", c => c.Int(nullable: false));
            AddColumn("dbo.Courses", "SchoolYear_SchoolYearId", c => c.Int(nullable: false));
            DropForeignKey("dbo.SignOnInformations", "Course_CourseId", "dbo.Courses");
            DropIndex("dbo.SignOnInformations", new[] { "Course_CourseId" });
            DropColumn("dbo.SignOnInformations", "Course_CourseId");
            CreateIndex("dbo.SignOnInformations", "Faculty_FacultyId");
            CreateIndex("dbo.Courses", "SchoolYear_SchoolYearId");
            AddForeignKey("dbo.SignOnInformations", "Faculty_FacultyId", "dbo.Faculties", "FacultyId", cascadeDelete: true);
            AddForeignKey("dbo.Courses", "SchoolYear_SchoolYearId", "dbo.SchoolYears", "SchoolYearId", cascadeDelete: true);
        }
    }
}
