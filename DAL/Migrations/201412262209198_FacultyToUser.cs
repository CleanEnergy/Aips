namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FacultyToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Faculty_FacultyId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Faculty_FacultyId");
            AddForeignKey("dbo.AspNetUsers", "Faculty_FacultyId", "dbo.Faculties", "FacultyId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Faculty_FacultyId", "dbo.Faculties");
            DropIndex("dbo.AspNetUsers", new[] { "Faculty_FacultyId" });
            DropColumn("dbo.AspNetUsers", "Faculty_FacultyId");
        }
    }
}
