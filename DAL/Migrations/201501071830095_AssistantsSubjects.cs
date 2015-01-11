namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssistantsSubjects : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssistantsSubjects",
                c => new
                    {
                        SubjectId = c.Int(nullable: false),
                        AssistantId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SubjectId, t.AssistantId })
                .ForeignKey("dbo.AspNetUsers", t => t.AssistantId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId)
                .Index(t => t.SubjectId)
                .Index(t => t.AssistantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssistantsSubjects", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.AssistantsSubjects", "AssistantId", "dbo.AspNetUsers");
            DropIndex("dbo.AssistantsSubjects", new[] { "AssistantId" });
            DropIndex("dbo.AssistantsSubjects", new[] { "SubjectId" });
            DropTable("dbo.AssistantsSubjects");
        }
    }
}
