namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateToUniversityEvents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UniversityEvents", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UniversityEvents", "Description");
        }
    }
}
