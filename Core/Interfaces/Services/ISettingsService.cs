using System.Collections.Generic;
using Core.Domain.Model;

namespace Core.Interfaces.Services
{
    public interface ISettingsService
    {
        bool AddMultipleSettings(IEnumerable<Settings> itemsToCreate);
    }
}