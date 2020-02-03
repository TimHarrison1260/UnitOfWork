namespace Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddScanResults : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArchiveScanResults",
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
                "dbo.ArchiveTestResults",
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
                .ForeignKey("dbo.ArchiveScanResults", t => t.ScanResultId, cascadeDelete: true)
                .Index(t => t.ScanResultId);
            
            CreateTable(
                "dbo.MemberProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        SurName = c.String(),
                        KnownAs = c.String(),
                        AvatarPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmailAccount = c.String(),
                        SlowResponseTime = c.Int(nullable: false),
                        ScanFrequency = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArchiveDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastRunDate = c.DateTime(),
                        LastArchiveDate = c.DateTime(),
                        MemberId = c.Int(nullable: false),
                        SettingsId = c.Int(nullable: false),
                        LastRunBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MemberProfiles", t => t.LastRunBy_Id)
                .ForeignKey("dbo.Settings", t => t.SettingsId, cascadeDelete: true)
                .Index(t => t.SettingsId)
                .Index(t => t.LastRunBy_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArchiveDetails", "SettingsId", "dbo.Settings");
            DropForeignKey("dbo.ArchiveDetails", "LastRunBy_Id", "dbo.MemberProfiles");
            DropForeignKey("dbo.ArchiveScanResults", "WebsiteId", "dbo.WebSites");
            DropForeignKey("dbo.ArchiveTestResults", "ScanResultId", "dbo.ArchiveScanResults");
            DropIndex("dbo.ArchiveDetails", new[] { "LastRunBy_Id" });
            DropIndex("dbo.ArchiveDetails", new[] { "SettingsId" });
            DropIndex("dbo.ArchiveTestResults", new[] { "ScanResultId" });
            DropIndex("dbo.ArchiveScanResults", new[] { "WebsiteId" });
            DropTable("dbo.ArchiveDetails");
            DropTable("dbo.Settings");
            DropTable("dbo.MemberProfiles");
            DropTable("dbo.ArchiveTestResults");
            DropTable("dbo.ArchiveScanResults");
        }
    }
}
