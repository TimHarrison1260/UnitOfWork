using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Model;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Interfaces.Factories;
using Infrastructure.Repositories;

namespace Infrastructure.Services
{
    /// <summary>
    /// Class <c>SiteMonitorService</c> provides the access to the underlying <see cref="SiteMonitorDbDataContext"/>.
    /// It derives from the <see cref="UnitOfWork{TEntity}"/>, where TEntity is the <see cref="SiteMonitorDbDataContext"/>
    /// and follows the UnitOfWork design pattern to allow control over transactional updates.
    /// </summary>
    /// <remarks>
    /// Private instances of ALL required repositories should be defined.  They MUST all use the same
    /// instance of <see cref="SiteMonitorDbDataContext"/>.
    /// </remarks>
    public class SiteMonitorService : UnitOfWork<SiteMonitorDbDataContext>, ISiteMonitorService
    {
        //  private instances of ALL required repositories
        private readonly SiteMonitorSettingsRepository _settingsRepository;
        private readonly ScanResultRepository _scanResultRepository;
        private readonly ArchiveScanResultRepository _archiveResultRepository;


        public SiteMonitorService(SiteMonitorDbDataContext context, IRepositoryFactory<SiteMonitorDbDataContext> repositoryFactory) 
            : base(context, repositoryFactory)
        {
            _settingsRepository = base.GetRepository<SiteMonitorSettingsRepository>();
            _scanResultRepository = base.GetRepository<ScanResultRepository>();
            _archiveResultRepository = base.GetRepository<ArchiveScanResultRepository>();
        }

        /// <summary>
        /// Get all scan results eligible for archiving
        /// </summary>
        /// <param name="archiveDate">Selecting scan results for archiving BEFORE this date</param>
        /// <returns>Collection of scan results for archiving</returns>
        public IEnumerable<ScanResult> GetScanResults(DateTime archiveDate)
        {
            var results = _scanResultRepository
                .Get(f => f.Scanned < archiveDate, 
                    o => o.OrderBy(r => r.Scanned), 
                    "TestResults");
            return results;
        }

        /// <summary>
        /// Gets the Site Monitor configuration settings.
        /// </summary>
        /// <returns>Instance of the current configuration settings</returns>
        public Settings GetSettings()
        {
            var settings = _settingsRepository.GetSettings();
            return settings;
        }


        /// <summary>
        /// Archives the scan results and individual tests prior to the
        /// specified date
        /// </summary>
        /// <param name="archiveScans">Scan Results to be archived.</param>
        /// <param name="settings">Site Monitor settings including archive run details</param>
        /// <returns>True if archive process is successfully completed, otherwise false</returns>
        public bool Archive(IEnumerable<ArchiveScanResult> archiveScans, Settings settings)
        {
            if (!archiveScans.Any()) return false;
            //  Transaction coding to update the database
            //  Add the ArchiveScanResults to the database
            //      including the archiveTestResults
            //  Remove the ScanResults with the same id's as the archiveScanResults from the database
            //      including the TestResults (cascade the delete??)
            //  Add the archiveDetails, in the settings to the settings in the database
            //  If all updates are successful, commit the updates
            //  return true if the commit is successful, otherwise return false
            //  Any exceptions, log the exception and return false.

            //  Before the model is passed into the transaction, where it will be updated and loose
            //  the original Id's preserve the Id's so the original ScanResults can be deleted
            var scanResultIds = archiveScans.Select(r => r.Id).ToList();

            //  Set up the transaction against the DbContext
            using (var archiveTxn = base.BeginTransaction())
            {
                try
                {
                    var success = true;
                    foreach (var archiveScanResult in archiveScans)
                    {
                        var result = _archiveResultRepository.Create(archiveScanResult);
                    }
                    //var saveResult = base.SaveChanges();
                    //success = (saveResult > 0);

                    //if (!success)
                    //{
                    //    var logEntry = "Unable to add archive scan results";
                    //    base.RollBack();
                    //    return false;
                    //}

                    //  OK to continue => Delete the scans from the ScanResults table
                    //  foreach (var archiveScanResult in archiveScans)
                    foreach (var id in scanResultIds)
                    {
                        var result = _scanResultRepository.Delete(id);
                    }
                    //saveResult = base.SaveChanges();
                    //success = (saveResult > 0);

                    //if (!success)
                    //{
                    //    var logEntry = "Unable to remove archived scan results";
                    //    base.RollBack();
                    //    return false;
                    //}

                    //  OK to continue => Add archive run details to settings.
                    success = _settingsRepository.Update(settings);

                    var saveResult = base.SaveChanges();
                    success = (saveResult > 0);

                    if (!success)
                    {
                        var logEntry = "Unable to add archive run details";
                        base.RollBack();
                        return false;
                    }

                    base.Commit();
                    return true;

                }
                catch (Exception ex)
                {
                    var logEntry = ex.Message;
                    base.RollBack();
                    return false;
                }

            }
        }
    }
}