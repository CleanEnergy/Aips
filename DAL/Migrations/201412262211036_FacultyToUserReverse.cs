namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FacultyToUserReverse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Faculty_FacultyId", "dbo.Faculties");
            DropIndex("dbo.AspNetUsers", new[] { "Faculty_FacultyId" });
            DropColumn("dbo.AspNetUsers", "Faculty_FacultyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Faculty_FacultyId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Faculty_FacultyId");
            AddForeignKey("dbo.AspNetUsers", "Faculty_FacultyId", "dbo.Faculties", "FacultyId");
        }
    }
}
