namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SurveyVisibility : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Surveys", "Visible", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Surveys", "Visible");
        }
    }
}
