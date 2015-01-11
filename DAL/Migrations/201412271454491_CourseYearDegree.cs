namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseYearDegree : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.Courses", "Degree", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Degree");
            DropColumn("dbo.Courses", "Year");
        }
    }
}
