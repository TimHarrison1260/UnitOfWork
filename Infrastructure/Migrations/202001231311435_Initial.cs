namespace Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WebSites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Url = c.String(),
                        Image = c.String(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScanResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        Scanned = c.DateTime(nullable: false),
                        WebsiteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WebSites", t => t.WebsiteId, cascadeDelete: true)
                .Index(t => t.WebsiteId);
            
            CreateTable(
                "dbo.TestResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Test = c.Int(nullable: false),
                        Url = c.String(),
                        HttpStatus = c.Int(),
                        Response = c.Single(nullable: false),
                        Checked = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        ScanResultId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ScanResults", t => t.ScanResultId, cascadeDelete: true)
                .Index(t => t.ScanResultId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScanResults", "WebsiteId", "dbo.WebSites");
            DropForeignKey("dbo.TestResults", "ScanResultId", "dbo.ScanResults");
            DropIndex("dbo.TestResults", new[] { "ScanResultId" });
            DropIndex("dbo.ScanResults", new[] { "WebsiteId" });
            DropTable("dbo.TestResults");
            DropTable("dbo.ScanResults");
            DropTable("dbo.WebSites");
        }
    }
}
