using Maxmod.Areas.Admin.ViewModels.Settings;
using Maxmod.Models;
using static Maxmod.Extensions.FileExtension;

namespace Maxmod.Services.Interfaces;

public interface ISettingsService
{
    Task<Settings> GetSettingsAsync();
    Task<FileValidationResult?> UpdateSettingsAsync(UpdateSettingsVM updateSettingsVM);
}
