namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseUpdate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Courses", "Year");
            DropColumn("dbo.Courses", "Degree");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "Degree", c => c.Int(nullable: false));
            AddColumn("dbo.Courses", "Year", c => c.Int(nullable: false));
        }
    }
}
