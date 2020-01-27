using System;
using System.Collections.Generic;
using System.Linq;
//using Core.Interfaces.Rules;

namespace Core.Domain.Model
{
    public class WebSite
    {
 
        /// <summary>
        /// Ctor: Parameterless 
        /// </summary>
        public WebSite(){}

   
        /// <summary>
        /// Get or set the id of the website
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get or set the Title of the Website
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get or set the Url of the Website
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Get ir set the Image of the Website home page
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Get or set the Type of the website
        /// </summary>
        public SiteTypeEnum Type { get; set; }

    


        /// <summary>
        /// Gets the results of each scan.
        /// </summary>
        public virtual IList<ScanResult> ScanResults { get; set; }

        /// <summary>
        /// Adds the result of a scan to the website
        /// </summary>
        /// <param name="result">Scan Results <see cref="ScanResult"/></param>
        public void AddScanResult(ScanResult result)
        {
            if (result != null)
                ScanResults.Add(result);
        }


    }
}
