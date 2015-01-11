namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SurveyQuestionTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SurveyQuestionTypes",
                c => new
                    {
                        TypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.TypeId);
            
            AddColumn("dbo.SurveyQuestions", "SurveyQuestionType_TypeId", c => c.Int());
            CreateIndex("dbo.SurveyQuestions", "SurveyQuestionType_TypeId");
            AddForeignKey("dbo.SurveyQuestions", "SurveyQuestionType_TypeId", "dbo.SurveyQuestionTypes", "TypeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SurveyQuestions", "SurveyQuestionType_TypeId", "dbo.SurveyQuestionTypes");
            DropIndex("dbo.SurveyQuestions", new[] { "SurveyQuestionType_TypeId" });
            DropColumn("dbo.SurveyQuestions", "SurveyQuestionType_TypeId");
            DropTable("dbo.SurveyQuestionTypes");
        }
    }
}
