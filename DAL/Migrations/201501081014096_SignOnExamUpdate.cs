namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SignOnExamUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SignOnExams", "IsSignedOff", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SignOnExams", "IsSignedOff");
        }
    }
}
