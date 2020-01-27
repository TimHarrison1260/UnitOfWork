using System.Collections.Generic;

namespace Core.Domain.Model
{
    public class Settings
    {
        /// <summary>
        /// Gets or sets a key for the settings to facilitate editing using Entity Framework
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get or set the email account to receive notifications of scan results
        /// </summary>
        public string EmailAccount { get; set; }

        /// <summary>
        /// Get or set the time limit in milli-seconds greater than which marks the response as slow.
        /// </summary>
        public int SlowResponseTime { get; set; }

        /// <summary>
        /// Get or set the frequency of the scans
        /// </summary>
        public FrequencyEnum ScanFrequency { get; set; }

        /// <summary>
        /// Gets or sets the details of the last Scan Result archive run
        /// </summary>
        public ICollection<ArchiveDetail> ArchiveRunDetails { get; set; }
    }
}
