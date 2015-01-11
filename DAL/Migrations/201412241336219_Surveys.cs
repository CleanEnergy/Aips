namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Surveys : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SurveyAnswers",
                c => new
                    {
                        SurveyAnswerId = c.Int(nullable: false, identity: true),
                        Answer = c.String(nullable: false, maxLength: 50),
                        SurveyQuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SurveyAnswerId)
                .ForeignKey("dbo.SurveyQuestions", t => t.SurveyQuestionId, cascadeDelete: true)
                .Index(t => t.SurveyQuestionId);
            
            CreateTable(
                "dbo.SurveyQuestions",
                c => new
                    {
                        SurveyQuestionId = c.Int(nullable: false, identity: true),
                        Question = c.String(nullable: false),
                        SurveyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SurveyQuestionId)
                .ForeignKey("dbo.Surveys", t => t.SurveyId, cascadeDelete: true)
                .Index(t => t.SurveyId);
            
            CreateTable(
                "dbo.Surveys",
                c => new
                    {
                        SurveyId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.SurveyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SurveyAnswers", "SurveyQuestionId", "dbo.SurveyQuestions");
            DropForeignKey("dbo.SurveyQuestions", "SurveyId", "dbo.Surveys");
            DropIndex("dbo.SurveyQuestions", new[] { "SurveyId" });
            DropIndex("dbo.SurveyAnswers", new[] { "SurveyQuestionId" });
            DropTable("dbo.Surveys");
            DropTable("dbo.SurveyQuestions");
            DropTable("dbo.SurveyAnswers");
        }
    }
}
