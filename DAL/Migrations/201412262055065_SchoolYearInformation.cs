namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SchoolYearInformation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SchoolYears",
                c => new
                    {
                        SchoolYearId = c.Int(nullable: false, identity: true),
                        YearStart = c.DateTime(nullable: false),
                        YearEnd = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SchoolYearId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Faculty_FacultyId = c.Int(nullable: false),
                        SchoolYear_SchoolYearId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.Faculties", t => t.Faculty_FacultyId, cascadeDelete: true)
                .ForeignKey("dbo.SchoolYears", t => t.SchoolYear_SchoolYearId, cascadeDelete: true)
                .Index(t => t.Faculty_FacultyId)
                .Index(t => t.SchoolYear_SchoolYearId);
            
            CreateTable(
                "dbo.Faculties",
                c => new
                    {
                        FacultyId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.FacultyId);
            
            CreateTable(
                "dbo.SchoolYearUsers",
                c => new
                    {
                        SchoolYear_SchoolYearId = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SchoolYear_SchoolYearId, t.User_Id })
                .ForeignKey("dbo.SchoolYears", t => t.SchoolYear_SchoolYearId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.SchoolYear_SchoolYearId)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.SignOnInformations", "Faculty_FacultyId", c => c.Int(nullable: false));
            AddColumn("dbo.SignOnInformations", "SchoolYear_SchoolYearId", c => c.Int(nullable: false));
            CreateIndex("dbo.SignOnInformations", "Faculty_FacultyId");
            CreateIndex("dbo.SignOnInformations", "SchoolYear_SchoolYearId");
            AddForeignKey("dbo.SignOnInformations", "Faculty_FacultyId", "dbo.Faculties", "FacultyId", cascadeDelete: true);
            AddForeignKey("dbo.SignOnInformations", "SchoolYear_SchoolYearId", "dbo.SchoolYears", "SchoolYearId", cascadeDelete: true);
            DropColumn("dbo.SignOnInformations", "Year");
            DropColumn("dbo.SignOnInformations", "Faculty");
            DropColumn("dbo.SignOnInformations", "Course");
            DropColumn("dbo.SignOnInformations", "Degree");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SignOnInformations", "Degree", c => c.String(nullable: false, maxLength: 5));
            AddColumn("dbo.SignOnInformations", "Course", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.SignOnInformations", "Faculty", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.SignOnInformations", "Year", c => c.Int(nullable: false));
            DropForeignKey("dbo.SignOnInformations", "SchoolYear_SchoolYearId", "dbo.SchoolYears");
            DropForeignKey("dbo.SignOnInformations", "Faculty_FacultyId", "dbo.Faculties");
            DropForeignKey("dbo.Courses", "SchoolYear_SchoolYearId", "dbo.SchoolYears");
            DropForeignKey("dbo.Courses", "Faculty_FacultyId", "dbo.Faculties");
            DropForeignKey("dbo.SchoolYearUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SchoolYearUsers", "SchoolYear_SchoolYearId", "dbo.SchoolYears");
            DropIndex("dbo.SchoolYearUsers", new[] { "User_Id" });
            DropIndex("dbo.SchoolYearUsers", new[] { "SchoolYear_SchoolYearId" });
            DropIndex("dbo.SignOnInformations", new[] { "SchoolYear_SchoolYearId" });
            DropIndex("dbo.SignOnInformations", new[] { "Faculty_FacultyId" });
            DropIndex("dbo.Courses", new[] { "SchoolYear_SchoolYearId" });
            DropIndex("dbo.Courses", new[] { "Faculty_FacultyId" });
            DropColumn("dbo.SignOnInformations", "SchoolYear_SchoolYearId");
            DropColumn("dbo.SignOnInformations", "Faculty_FacultyId");
            DropTable("dbo.SchoolYearUsers");
            DropTable("dbo.Faculties");
            DropTable("dbo.Courses");
            DropTable("dbo.SchoolYears");
        }
    }
}
