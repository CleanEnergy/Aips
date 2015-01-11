namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SurveyQuestionsTypeId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SurveyQuestions", "SurveyQuestionType_TypeId", "dbo.SurveyQuestionTypes");
            DropIndex("dbo.SurveyQuestions", new[] { "SurveyQuestionType_TypeId" });
            RenameColumn(table: "dbo.SurveyQuestions", name: "SurveyQuestionType_TypeId", newName: "SurveyQuestionTypeId");
            AlterColumn("dbo.SurveyQuestions", "SurveyQuestionTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.SurveyQuestions", "SurveyQuestionTypeId");
            AddForeignKey("dbo.SurveyQuestions", "SurveyQuestionTypeId", "dbo.SurveyQuestionTypes", "TypeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SurveyQuestions", "SurveyQuestionTypeId", "dbo.SurveyQuestionTypes");
            DropIndex("dbo.SurveyQuestions", new[] { "SurveyQuestionTypeId" });
            AlterColumn("dbo.SurveyQuestions", "SurveyQuestionTypeId", c => c.Int());
            RenameColumn(table: "dbo.SurveyQuestions", name: "SurveyQuestionTypeId", newName: "SurveyQuestionType_TypeId");
            CreateIndex("dbo.SurveyQuestions", "SurveyQuestionType_TypeId");
            AddForeignKey("dbo.SurveyQuestions", "SurveyQuestionType_TypeId", "dbo.SurveyQuestionTypes", "TypeId");
        }
    }
}
