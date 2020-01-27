using System;
using System.Net;

namespace Core.Domain.Model
{
    public class TestResult
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of test
        /// </summary>
        public TestTypeEnum Test { get; set; }

        /// <summary>
        /// Get or set the Url of the page
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or set the Http Status code returned from the page
        /// </summary>
        /// <remarks>This is nullable, since it doesn't apply to all tests.</remarks>
        public HttpStatusCode? HttpStatus { get; set; }

        /// <summary>
        /// Get or set the response time or the page request
        /// </summary>
        public float Response { get; set; }

        /// <summary>
        /// Get or set the date and time the page was checked
        /// </summary>
        public DateTime Checked { get; set; }

        /// <summary>
        /// Gets or sets the status of the test
        /// </summary>
        public StatusEnum Status { get; set; }


        public virtual ScanResult ScanResult { get; set; }

        public int ScanResultId { get; set; }
    }
}
