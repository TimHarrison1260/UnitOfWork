using System;

namespace Core.Domain.Model
{
    public class ArchiveDetail
    {
        /// <summary>
        /// Get or Set the id of the Archive Detail
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get or Set the date the last archive was run
        /// </summary>
        public DateTime? LastRunDate { get; set; }

        /// <summary>
        /// Get or Set the Archive Date use in the last archive run
        /// </summary>
        public DateTime? LastArchiveDate { get; set; }

        /// <summary>
        /// Get or Set the member who ran the last archive.
        /// Empty indicates the system ran an auto archive
        /// </summary>
        public virtual MemberProfile LastRunBy { get; set; }

        public int MemberId { get; set; }

        public virtual Settings SettingsConfig { get; set; }

        public int SettingsId { get; set; }
    }
}
