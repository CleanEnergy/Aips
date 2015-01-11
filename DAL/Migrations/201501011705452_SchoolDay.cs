namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SchoolDay : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SchoolDays",
                c => new
                    {
                        SchoolDayId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 15),
                    })
                .PrimaryKey(t => t.SchoolDayId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SchoolDays");
        }
    }
}
