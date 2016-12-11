namespace PictureTagger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LongerFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pictures", "FileType", c => c.String(nullable: false, unicode: false));
            AlterColumn("dbo.Pictures", "Name", c => c.String(nullable: false, maxLength: 60, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pictures", "Name", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Pictures", "FileType", c => c.String(nullable: false, maxLength: 4, unicode: false));
        }
    }
}
