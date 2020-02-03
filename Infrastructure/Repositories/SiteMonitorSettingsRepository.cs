using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Model;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Data;
using Infrastructure.Interfaces.Data;

namespace Infrastructure.Repositories
{
    public class SiteMonitorSettingsRepository : Repository<Settings, SiteMonitorDbDataContext>, ISiteMonitorSettingsRepository
    {
        //private readonly SiteMonitorDbDataContext _db;

        public SiteMonitorSettingsRepository(){}

        public SiteMonitorSettingsRepository(SiteMonitorDbDataContext context) : base(context)
        {
        }

        //  Preserve this ctor, to allow the existing code to reference until completely refactored.
        //  It allows for the repository to be injected by the IoC mapped to the ISiteMonitorSettingsRepository interface
        //  The above ctor's are for use with services that implement the UnitOfWork abstract class, which instantiate the repository using the previous ctor.
        //
        //  Used via the ScanController, GetSettingsAsync & from retrieving frequency in domain rules (??)
        //  TODO: Investigate the domain rule, for correct configuration as it shouldn't re-instantiate it.
        //  
        //  This calls a 3rd ctor in the base class that takes an ISession<> object for the DbContext, in which
        //  it sets the boolean "callSaveChanges", allowing the repository to be used without being used by
        //  the UnitOfWork class.  (i.e., as I've always used the Repository until now).
        public SiteMonitorSettingsRepository(ISession<SiteMonitorDbDataContext> siteMonitorDataContext) : base(siteMonitorDataContext)
        {
            //_db = siteMonitorDataContext as SiteMonitorDbDataContext ?? throw new ArgumentNullException(nameof(siteMonitorDataContext), "No valid context supplied");
        }

        /// <summary>
        /// Get the configuration settings for the site monitor
        /// </summary>
        /// <returns></returns>
        public Settings GetSettings()
        {
            //var settings = _db.MonitorSettings.FirstOrDefault();
            var settings = Db.MonitorSettings.FirstOrDefault();
            return settings;
        }

        /// <summary>
        /// Get the configuration settings for the site monitor (Asynchronous)
        /// </summary>
        /// <returns></returns>
        public async Task<Settings> GetSettingsAsync()
        {
            //var settings = await _db.MonitorSettings.FirstOrDefaultAsync();
            var settings = await Db.MonitorSettings.FirstOrDefaultAsync();
            return settings;
        }

        public ArchiveDetail GetLatestArchiveDetails()
        {
            var lastArchiveDetail = Db.MonitorSettings
                .Include(s => s.ArchiveRunDetails)
                .SelectMany(s => s.ArchiveRunDetails)
                .OrderByDescending(t => t.LastArchiveDate)
                .FirstOrDefault();
            return lastArchiveDetail;
        }

        public IEnumerable<ArchiveDetail> GetArchiveDetails()
        {
            var archiveRunDetails = Db.MonitorSettings
                .Include(s => s.ArchiveRunDetails)
                .SelectMany(s => s.ArchiveRunDetails)
                .OrderByDescending(t => t.LastRunDate)
                .ToList();  //  Execute query
            return archiveRunDetails;
        }

        /// <summary>
        /// Update the settings, and ADD the instance of the ArchiveRunDetails
        /// </summary>
        /// <param name="model">Updated settings instance, including a ArchiveRunDetail to add.</param>
        /// <returns></returns>
        public override bool Update(Settings model)
        {
            base.Update(model);

            foreach (var archiveDetail in model.ArchiveRunDetails)
            {
                Db.Entry(archiveDetail).State = EntityState.Added;
            }

            return true;
        }

    }
}
