namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Lectures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lectures",
                c => new
                    {
                        LectureId = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Day = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LectureId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lectures", "SubjectId", "dbo.Subjects");
            DropIndex("dbo.Lectures", new[] { "SubjectId" });
            DropTable("dbo.Lectures");
        }
    }
}
