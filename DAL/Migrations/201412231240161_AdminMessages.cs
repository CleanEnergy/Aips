namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminMessages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminMessages",
                c => new
                    {
                        AdminMessageId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        PostedOn = c.DateTime(nullable: false),
                        Admin_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AdminMessageId)
                .ForeignKey("dbo.AspNetUsers", t => t.Admin_Id)
                .Index(t => t.Admin_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdminMessages", "Admin_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AdminMessages", new[] { "Admin_Id" });
            DropTable("dbo.AdminMessages");
        }
    }
}
