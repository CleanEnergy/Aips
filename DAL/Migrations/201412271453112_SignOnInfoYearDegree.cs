namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SignOnInfoYearDegree : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SignOnInformations", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.SignOnInformations", "Degree", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SignOnInformations", "Degree");
            DropColumn("dbo.SignOnInformations", "Year");
        }
    }
}
