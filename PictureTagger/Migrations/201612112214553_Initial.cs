namespace PictureTagger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        PictureID = c.Int(nullable: false, identity: true),
                        OwnerID = c.String(nullable: false, maxLength: 128),
                        Data = c.Binary(nullable: false),
                        FileType = c.String(nullable: false, maxLength: 4, unicode: false),
                        Name = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.PictureID);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagID = c.Int(nullable: false, identity: true),
                        Tag = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.TagID);
            
            CreateTable(
                "dbo.PictureTags",
                c => new
                    {
                        PictureID = c.Int(nullable: false),
                        TagID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PictureID, t.TagID })
                .ForeignKey("dbo.Pictures", t => t.PictureID, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagID, cascadeDelete: true)
                .Index(t => t.PictureID)
                .Index(t => t.TagID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PictureTags", "TagID", "dbo.Tags");
            DropForeignKey("dbo.PictureTags", "PictureID", "dbo.Pictures");
            DropIndex("dbo.PictureTags", new[] { "TagID" });
            DropIndex("dbo.PictureTags", new[] { "PictureID" });
            DropTable("dbo.PictureTags");
            DropTable("dbo.Tags");
            DropTable("dbo.Pictures");
        }
    }
}
