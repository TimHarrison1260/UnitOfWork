using System.Collections.Generic;
using System.Linq;
using Core.Domain.Model;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Interfaces.Factories;
using Infrastructure.Repositories;

namespace Infrastructure.Services
{
    public class SettingsService: UnitOfWork<SiteMonitorDbDataContext>, ISettingsService
    {
        private readonly SiteMonitorSettingsRepository _settingsRepository;

        public SettingsService(SiteMonitorDbDataContext context, IRepositoryFactory<SiteMonitorDbDataContext> repositoryFactory) : base(context, repositoryFactory)
        {
            _settingsRepository = base.GetRepository<SiteMonitorSettingsRepository>();
        }

        public bool AddMultipleSettings(IEnumerable<Settings> itemsToCreate)
        {
            var settings = itemsToCreate.ToList();
            if (settings.Count() != 2) return false;

            var success = false;

            //  using (var txn = Context.Database.BeginTransaction())
            using (var txn = base.BeginTransaction())
            {
                try
                {
                    //  Add the first setting
                    var result = _settingsRepository.Create(settings[0]);
                    var res1 =  base.SaveChanges();
                    success = (res1 > 0);

                    if (success)
                    {
                        //  Insert settings2 record  if first was ok
                        _settingsRepository.Create(settings[1]);
                        var res2 = base.SaveChanges();
                        success = res2 > 0;
                    }

                    if (success)
                    {
                        //  txn.Commit();
                        base.Commit();
                    }
                    else
                    {
                        //  txn.Rollback();
                        base.RollBack();
                    }
                }
                catch
                {
                    //  txn.Rollback();
                    base.RollBack();
                }

            }

            return success;
        }
    }
}