using System.Collections.Generic;
using System.Net;
using Core.Domain.Model;

namespace Infrastructure.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Infrastructure.Data.SiteMonitorDbDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Infrastructure.Data.SiteMonitorDbDataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            var site1 = new WebSite()
            {
                Id=1,
                Image = "",
                Title = "First Web Site",
                Type = SiteTypeEnum.Development,
                Url = "http://firstsite.com",
                ScanResults = new List<ScanResult>()
            };

            var scan1 = new ScanResult()
            {
                Id = 1,
                Scanned = new DateTime(2020, 1, 1),
                Url = "",
                TestResults = new List<TestResult>()
                {
                    new TestResult()
                    {
                        Id = 1,
                        Checked = new DateTime(2020, 1, 1),
                        HttpStatus = HttpStatusCode.OK,
                        Response = 20F,
                        Status = StatusEnum.Healthy,
                        Test = TestTypeEnum.PageTest,
                        Url = "http://firstsite.com/page1"
                    },
                    new TestResult()
                    {
                        Id = 2,
                        Checked = new DateTime(2020, 1, 1),
                        HttpStatus = HttpStatusCode.OK,
                        Response = 20F,
                        Status = StatusEnum.Healthy,
                        Test = TestTypeEnum.PageTest,
                        Url = "http://firstsite.com/page2"
                    }
                }
            };

            site1.AddScanResult(scan1);

            context.WebSites.AddOrUpdate(site1);

        }
    }
}
