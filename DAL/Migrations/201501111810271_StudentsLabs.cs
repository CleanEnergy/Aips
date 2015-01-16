namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsLabs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentsLabs",
                c => new
                    {
                        StudentsLabsId = c.Int(nullable: false, identity: true),
                        LabId = c.Int(nullable: false),
                        StudentId = c.String(nullable: false, maxLength: 128),
                        WasPresent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StudentsLabsId)
                .ForeignKey("dbo.Labs", t => t.LabId)
                .ForeignKey("dbo.AspNetUsers", t => t.StudentId)
                .Index(t => t.LabId)
                .Index(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentsLabs", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentsLabs", "LabId", "dbo.Labs");
            DropIndex("dbo.StudentsLabs", new[] { "StudentId" });
            DropIndex("dbo.StudentsLabs", new[] { "LabId" });
            DropTable("dbo.StudentsLabs");
        }
    }
}
