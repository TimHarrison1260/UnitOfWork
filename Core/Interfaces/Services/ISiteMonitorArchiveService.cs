using System;
using Core.Domain.Model;

namespace Core.Interfaces.Services
{
    public interface ISiteMonitorArchiveService
    {
        bool Archive(DateTime archiveDate, MemberProfile memberProfile);
    }
}