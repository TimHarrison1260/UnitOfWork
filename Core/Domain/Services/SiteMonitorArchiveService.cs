using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Model;
using Core.Interfaces.Mappers;
using Core.Interfaces.Services;

namespace Core.Domain.Services
{
    /// <summary>
    /// Class <c>SiteMonitorArchiveService</c> is responsible for archiving <see cref="ScanResult"/>
    /// to the <see cref="ArchiveScanResult"/> older than a specified date.
    /// </summary>
    /// <remarks>
    /// It selects ScanResults that are eligible for archiving, copies them to ArchiveScanResults,
    /// and updates the <see cref="Settings"/> by adding a <see cref="ArchiveDetail"/> object recording
    /// when this archive run was performed.  Lastly, it calls the <see cref="ISiteMonitorService"/> to
    /// persists these changes to the Domain objects in the underlying storage media.
    /// </remarks>
    public class SiteMonitorArchiveService: ISiteMonitorArchiveService
    {
        private readonly ISiteMonitorService _siteMonitorService;

        private readonly IMapper<ScanResult, ArchiveScanResult> _scanResultMapper;

        public SiteMonitorArchiveService(ISiteMonitorService siteMonitorService, IMapper<ScanResult, ArchiveScanResult> scanResultMapper)
        {
            _siteMonitorService = siteMonitorService ?? throw new ArgumentNullException(nameof(siteMonitorService));
            _scanResultMapper = scanResultMapper ?? throw new ArgumentNullException(nameof(scanResultMapper));
        }

        /// <summary>
        /// Archives <see cref="ScanResult"/>s older than the specified Date, recording
        /// that it has been done by <see cref="MemberProfile"/>.
        /// </summary>
        /// <param name="archiveDate">Archive results older than this date</param>
        /// <param name="memberProfile">Member performing the archive</param>
        /// <returns>True if successfully archived otherwise False.</returns>
        public bool Archive(DateTime archiveDate, MemberProfile memberProfile)
        {
            //  1.  Get all scan results that are dated before the archiveDate, including individual test results
            var eligibleScans = _siteMonitorService.GetScanResults(archiveDate).ToList();
            if (!eligibleScans.Any()) return false;

            //  2.  Map all scan results to archive scan results
            var archiveScans = _scanResultMapper.Map(eligibleScans);

            //  2a. Create Archive Details and attach to Settings
            var settings = _siteMonitorService.GetSettings();   //This doesn't get any existing archiveDetails
            //  TODO: Convert to factory class
            var archiveDetails = new ArchiveDetail()
            {
                Id = 0,
                LastArchiveDate = archiveDate,
                LastRunDate = DateTime.Now,
                LastRunBy = memberProfile,
                MemberId = memberProfile.Id,
                SettingsConfig = settings,
                SettingsId = settings.Id
            };

            //  Set the collection of archive details to new list if it's null => should always be the case here.
            if (settings.ArchiveRunDetails == null) settings.ArchiveRunDetails = new List<ArchiveDetail>();

            settings.ArchiveRunDetails.Add(archiveDetails);

            //  3.  Call service to persist changes 
            var success = _siteMonitorService.Archive(archiveScans, settings);

            //  4.  return the result / status of the archive persistence
            return success;
        }

    }
}