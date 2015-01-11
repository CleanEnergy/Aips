namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExamCopies : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExamCopies",
                c => new
                    {
                        ExamCopyId = c.Int(nullable: false, identity: true),
                        ExamId = c.Int(nullable: false),
                        StudentId = c.String(nullable: false, maxLength: 128),
                        ExamImage = c.Binary(),
                    })
                .PrimaryKey(t => t.ExamCopyId)
                .ForeignKey("dbo.Exams", t => t.ExamId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.ExamId)
                .Index(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExamCopies", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ExamCopies", "ExamId", "dbo.Exams");
            DropIndex("dbo.ExamCopies", new[] { "StudentId" });
            DropIndex("dbo.ExamCopies", new[] { "ExamId" });
            DropTable("dbo.ExamCopies");
        }
    }
}
