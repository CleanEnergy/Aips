namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsLabsDateUpdate1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.StudentsLabs");
            AddPrimaryKey("dbo.StudentsLabs", new[] { "LabId", "Date", "StudentId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.StudentsLabs");
            AddPrimaryKey("dbo.StudentsLabs", new[] { "LabId", "Date" });
        }
    }
}
