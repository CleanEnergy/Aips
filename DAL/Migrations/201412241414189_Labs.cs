namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Labs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Labs",
                c => new
                    {
                        LabId = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        ScheduledOn = c.DateTime(nullable: false),
                        MinimalPresencePercent = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LabId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Labs", "SubjectId", "dbo.Subjects");
            DropIndex("dbo.Labs", new[] { "SubjectId" });
            DropTable("dbo.Labs");
        }
    }
}
