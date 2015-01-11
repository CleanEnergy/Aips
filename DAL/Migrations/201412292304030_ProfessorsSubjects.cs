namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfessorsSubjects : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfessorsSubjects",
                c => new
                    {
                        SubjectId = c.Int(nullable: false),
                        ProfessorId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SubjectId, t.ProfessorId })
                .ForeignKey("dbo.AspNetUsers", t => t.ProfessorId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId)
                .Index(t => t.ProfessorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProfessorsSubjects", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.ProfessorsSubjects", "ProfessorId", "dbo.AspNetUsers");
            DropIndex("dbo.ProfessorsSubjects", new[] { "ProfessorId" });
            DropIndex("dbo.ProfessorsSubjects", new[] { "SubjectId" });
            DropTable("dbo.ProfessorsSubjects");
        }
    }
}
