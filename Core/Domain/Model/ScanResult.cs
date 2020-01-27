using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.Model
{
    public class ScanResult
    {
        /// <summary>
        /// Get or set the Id of the site being checked
        /// </summary>
        public int Id { get; set; }

        ///// <summary>
        ///// Get or set the Title of the site being checked
        ///// </summary>
        //public string Title { get; set; }

        /// <summary>
        /// Get or set the Url of the site.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the date and time the scan was performed
        /// </summary>
        public DateTime Scanned { get; set; }

        /// <summary>
        /// Get the overall status of the site.
        /// </summary>
        public StatusEnum Status
        {
            get
            {
                if (TestResults == null || !TestResults.Any()) return StatusEnum.NotChecked;

                var failures = TestResults.Where(r => r.Status == StatusEnum.Failure);
                if (failures.Any()) return StatusEnum.Failure;
                var warnings = TestResults.Where(r => r.Status == StatusEnum.Warnings);
                if (warnings.Any()) return StatusEnum.Warnings;
                return StatusEnum.Healthy;
            }
        }

        public string StatusName
        {
            get
            {
                var statusName = Enum.GetName(typeof(StatusEnum), Status);
                return statusName.ToLower();
            }
        }

        /// <summary>
        /// Get or set the results of each individual pages
        /// </summary>
        public ICollection<TestResult> TestResults { get; set; }


        public virtual WebSite WebSite { get; set; }

        public int WebsiteId { get; set; }


    }
}
