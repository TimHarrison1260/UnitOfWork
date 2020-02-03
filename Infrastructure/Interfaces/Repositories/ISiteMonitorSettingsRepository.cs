using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Model;

namespace Infrastructure.Interfaces.Repositories
{
    public interface ISiteMonitorSettingsRepository
    {
        /// <summary>
        /// Get the configuration settings for the site monitor
        /// </summary>
        /// <returns></returns>
        Settings GetSettings();

        Task<Settings> GetSettingsAsync();

        ArchiveDetail GetLatestArchiveDetails();
        IEnumerable<ArchiveDetail> GetArchiveDetails();


    }
}