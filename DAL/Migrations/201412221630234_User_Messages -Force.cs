namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_MessagesForce : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserMessages", "SenderUsername", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.UserMessages", "RecepientUsername", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserMessages", "RecepientUsername", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.UserMessages", "SenderUsername", c => c.String(nullable: false, maxLength: 256));
        }
    }
}
