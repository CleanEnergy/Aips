namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsLabsDateUpdate : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.StudentsLabs");
            AddColumn("dbo.StudentsLabs", "Date", c => c.DateTime(nullable: false));
            AddPrimaryKey("dbo.StudentsLabs", new[] { "LabId", "Date" });
            DropColumn("dbo.StudentsLabs", "StudentsLabsId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StudentsLabs", "StudentsLabsId", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.StudentsLabs");
            DropColumn("dbo.StudentsLabs", "Date");
            AddPrimaryKey("dbo.StudentsLabs", "StudentsLabsId");
        }
    }
}
