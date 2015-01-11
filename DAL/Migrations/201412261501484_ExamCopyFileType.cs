namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExamCopyFileType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExamCopies", "FileType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExamCopies", "FileType");
        }
    }
}
