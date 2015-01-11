namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileTypeToMimeType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExamCopies", "MimeType", c => c.String(nullable: false));
            DropColumn("dbo.ExamCopies", "FileType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ExamCopies", "FileType", c => c.String(nullable: false));
            DropColumn("dbo.ExamCopies", "MimeType");
        }
    }
}
