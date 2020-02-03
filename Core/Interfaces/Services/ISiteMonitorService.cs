using System;
using System.Collections.Generic;
using Core.Domain.Model;

namespace Core.Interfaces.Services
{
    /// <summary>
    /// Interface <c>ISiteMonitorService</c> defines the contract for the
    /// Service accessing the SiteMonitor context
    /// </summary>
    public interface ISiteMonitorService
    {
        /// <summary>
        /// Get all scan results eligible for archiving
        /// </summary>
        /// <param name="archiveDate">Date for selecting scan results for archiving</param>
        /// <returns>Collection of scan results for archiving</returns>
        IEnumerable<ScanResult> GetScanResults(DateTime archiveDate);

        /// <summary>
        /// Gets the Site Monitor configuration settings.
        /// </summary>
        /// <returns>Instance of the current configuration settings</returns>
        Settings GetSettings();

        /// <summary>
        /// Archives the scan results and individual tests prior to the
        /// specified date
        /// </summary>
        /// <param name="archiveScans">Scan Results to be archived.</param>
        /// <param name="settings">Site Monitor settings including archive run details</param>
        /// <returns>True if archive process is successfully completed, otherwise false</returns>
        bool Archive(IEnumerable<ArchiveScanResult> archiveScans, Settings settings);
    }
}