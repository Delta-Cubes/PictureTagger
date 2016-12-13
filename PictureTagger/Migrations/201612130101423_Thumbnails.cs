namespace PictureTagger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Thumbnails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pictures", "ThumbnailData", c => c.Binary(nullable: false));
            AddColumn("dbo.Pictures", "Hash", c => c.String(nullable: false));
            DropColumn("dbo.Pictures", "Data");
            DropColumn("dbo.Pictures", "FileType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pictures", "FileType", c => c.String(nullable: false, unicode: false));
            AddColumn("dbo.Pictures", "Data", c => c.Binary(nullable: false));
            DropColumn("dbo.Pictures", "Hash");
            DropColumn("dbo.Pictures", "ThumbnailData");
        }
    }
}
