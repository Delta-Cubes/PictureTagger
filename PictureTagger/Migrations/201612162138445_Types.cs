namespace PictureTagger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Types : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pictures", "Filesize", c => c.Int(nullable: false));
            AddColumn("dbo.Pictures", "OriginalType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pictures", "OriginalType");
            DropColumn("dbo.Pictures", "Filesize");
        }
    }
}
