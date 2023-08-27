using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using HappyReset.Settings.Services;
using HappyReset.Features.Shared.Services;
using Blish_HUD.GameIntegration;
using Gw2Sharp.WebApi.V2.Models;
using System.Collections.Generic;

namespace HappyReset;


[Export(typeof(Blish_HUD.Modules.Module))]
public class Module : Blish_HUD.Modules.Module
{
    public static string DIRECTORY_PATH = "happyReset"; //Defined folder in manifest.json

    internal static readonly Logger ModuleLogger = Logger.GetLogger<Module>();

    [ImportingConstructor]
    public Module([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters)
    {
        Service.ModuleInstance = this;
        Service.ContentsManager = moduleParameters.ContentsManager;
        Service.Gw2ApiManager = moduleParameters.Gw2ApiManager;
        Service.DirectoriesManager = moduleParameters.DirectoriesManager;
    }

    protected override void DefineSettings(SettingCollection settings) => Service.Settings = new SettingService(settings);

    public override IView GetSettingsView() => new Settings.Views.ModuleMainSettingsView();

    protected override void Initialize()
    {
        this.TEMP_FIX_SetTacOAsActive();
    }

    protected override Task LoadAsync()
    {

        Service.Gw2ApiManager.SubtokenUpdated += Gw2ApiManager_SubtokenUpdated;

        Service.Persistance = Persistance.Load();

        Service.Textures = new TextureService(Service.ContentsManager);

        Service.ResetWatcher = new ResetsWatcherService();

        Service.BouncyChestControl = new();

        

        return Task.CompletedTask;

        /*GameService.Overlay.UserLocaleChanged += (s, e) =>
        {
           //todo: refresh views
        };*/


    }

    private void TEMP_FIX_SetTacOAsActive()
    {
        // SOTO Fix
        if (Program.OverlayVersion < new SemVer.Version(1, 1, 0))
        {
            try
            {
                var tacoActive = typeof(TacOIntegration).GetProperty(nameof(TacOIntegration.TacOIsRunning)).GetSetMethod(true);
                tacoActive?.Invoke(GameService.GameIntegration.TacO, new object[] { true });
            }
            catch { /* NOOP */ }
        }
    }

    protected override void Unload()
    {
        Service.Gw2ApiManager.SubtokenUpdated -= Gw2ApiManager_SubtokenUpdated;

        Service.BouncyChestControl?.Dispose();

        Service.ContentsManager?.Dispose();
        Service.Textures?.Dispose();
        Service.ResetWatcher?.Dispose();
    }

    protected override void Update(GameTime gameTime)
    {
        Service.ResetWatcher?.Update(gameTime);
        Service.BouncyChestControl?.Update();
    }

    private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
    {
        Task.Run(async () =>
        {
            Service.CurrentAccountName = await AccountNameService.UpdateAccountName();
            Service.BouncyChestControl.AccountNameChanged();
        });
    }

}