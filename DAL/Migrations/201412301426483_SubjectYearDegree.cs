namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubjectYearDegree : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subjects", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.Subjects", "Degree", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subjects", "Degree");
            DropColumn("dbo.Subjects", "Year");
        }
    }
}
