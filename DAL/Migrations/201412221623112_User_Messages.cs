namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_Messages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserMessages",
                c => new
                    {
                        UserMessageId = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false, maxLength: 50),
                        Content = c.String(nullable: false),
                        SentOn = c.DateTime(nullable: false),
                        SenderUsername = c.String(nullable: false, maxLength: 30),
                        RecepientUsername = c.String(nullable: false, maxLength: 30),
                        IsTrash = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserMessageId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserMessages");
        }
    }
}
