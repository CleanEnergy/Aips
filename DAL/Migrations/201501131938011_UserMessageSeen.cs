namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserMessageSeen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserMessages", "Seen", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserMessages", "Seen");
        }
    }
}
