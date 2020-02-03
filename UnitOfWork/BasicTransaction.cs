using System.Collections.Generic;
using Core.Domain.Model;
using Infrastructure.Data;

namespace UnitOfWork
{
    public class BasicTransaction
    {
        public bool Execute()
        {
            var context = new SiteMonitorDbDataContext();

            var settings1 = new Settings()
            {
                Id=0,
                EmailAccount = "tim@dev.com",
                SlowResponseTime = 1000,
                ScanFrequency = FrequencyEnum.Daily,
                ArchiveRunDetails = new List<ArchiveDetail>()
            };

            var settings2 = new Settings()
            {
                Id=2,
                EmailAccount = "tim@dev.com",
                SlowResponseTime = 1000,
                ScanFrequency = FrequencyEnum.Daily,
                ArchiveRunDetails = null
            };

            var success = false;

            using (var txn = context.Database.BeginTransaction())
            {
                try
                {
                    //  Insert settings1 record
                    context.MonitorSettings.Add(settings1);

                    var res1 =  context.SaveChanges();
                    success = (res1 > 0);


                    if (success)
                    {
                        //  Insert settings2 record  if first was ok
                        context.MonitorSettings.Add(settings2);
                        var res2 = context.SaveChanges();
                        success = false; //res2 > 0;
                    }

                    if (success)
                    {
                        txn.Commit();
                    }
                    else
                    {
                        txn.Rollback();
                    }
                }
                catch
                {
                    txn.Rollback();
                }

            }

            return success;
        }
    }
}