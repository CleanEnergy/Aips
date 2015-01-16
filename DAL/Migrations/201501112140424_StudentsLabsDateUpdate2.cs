namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsLabsDateUpdate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StudentsLabs", "LabId", "dbo.Labs");
            DropIndex("dbo.StudentsLabs", new[] { "LabId" });
            DropPrimaryKey("dbo.StudentsLabs");
            CreateTable(
                "dbo.ScheduledLabs",
                c => new
                    {
                        ScheduledLabId = c.Int(nullable: false, identity: true),
                        LabId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ScheduledLabId)
                .ForeignKey("dbo.Labs", t => t.LabId)
                .Index(t => t.LabId);
            
            AddColumn("dbo.StudentsLabs", "ScheduledLabId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.StudentsLabs", new[] { "ScheduledLabId", "StudentId" });
            CreateIndex("dbo.StudentsLabs", "ScheduledLabId");
            AddForeignKey("dbo.StudentsLabs", "ScheduledLabId", "dbo.ScheduledLabs", "ScheduledLabId");
            DropColumn("dbo.StudentsLabs", "LabId");
            DropColumn("dbo.StudentsLabs", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StudentsLabs", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.StudentsLabs", "LabId", c => c.Int(nullable: false));
            DropForeignKey("dbo.StudentsLabs", "ScheduledLabId", "dbo.ScheduledLabs");
            DropForeignKey("dbo.ScheduledLabs", "LabId", "dbo.Labs");
            DropIndex("dbo.ScheduledLabs", new[] { "LabId" });
            DropIndex("dbo.StudentsLabs", new[] { "ScheduledLabId" });
            DropPrimaryKey("dbo.StudentsLabs");
            DropColumn("dbo.StudentsLabs", "ScheduledLabId");
            DropTable("dbo.ScheduledLabs");
            AddPrimaryKey("dbo.StudentsLabs", new[] { "LabId", "Date", "StudentId" });
            CreateIndex("dbo.StudentsLabs", "LabId");
            AddForeignKey("dbo.StudentsLabs", "LabId", "dbo.Labs", "LabId");
        }
    }
}
