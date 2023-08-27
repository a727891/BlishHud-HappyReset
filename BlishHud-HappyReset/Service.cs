using Blish_HUD.Modules.Managers;
using HappyReset.Features.Shared.Services;
using HappyReset.Settings.Services;

namespace HappyReset;

public static class Service 
{
    public static string CurrentAccountName { get; set; } = AccountNameService.DEFAULT_ACCOUNT_NAME;
    public static Module ModuleInstance { get; set; } = null!;
    public static SettingService Settings { get; set; } = null!;
    public static ContentsManager ContentsManager { get; set; } = null!;
    public static Gw2ApiManager Gw2ApiManager { get; set; } = null!;
    public static DirectoriesManager DirectoriesManager { get; set; } = null!;
    public static TextureService? Textures { get; set; }

    public static Persistance Persistance { get; set; } = null!;

    public static ResetsWatcherService ResetWatcher { get; set; } = null!;

}