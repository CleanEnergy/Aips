namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinishedSurveys : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FinishedSurveys",
                c => new
                    {
                        FinishedSurveyId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        SurveyId = c.Int(nullable: false),
                        AnswerIds = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.FinishedSurveyId)
                .ForeignKey("dbo.Surveys", t => t.SurveyId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.SurveyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FinishedSurveys", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FinishedSurveys", "SurveyId", "dbo.Surveys");
            DropIndex("dbo.FinishedSurveys", new[] { "SurveyId" });
            DropIndex("dbo.FinishedSurveys", new[] { "UserId" });
            DropTable("dbo.FinishedSurveys");
        }
    }
}
