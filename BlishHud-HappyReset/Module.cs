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
using HappyReset.Features.BouncyChest.Controls;
using Blish_HUD.Input;
using HappyReset.Utils;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace HappyReset;


[Export(typeof(Blish_HUD.Modules.Module))]
public class Module : Blish_HUD.Modules.Module
{
    public static string DIRECTORY_PATH = "happyReset"; //Defined folder in manifest.json

    internal static readonly Logger ModuleLogger = Logger.GetLogger<Module>();

    private BouncyNotification _bouncyChest;

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

        _bouncyChest = new BouncyNotification(
            Service.Textures.DailyChestIcon,
            Service.Textures.DailyChestIcon
        )
        {
            Parent = GameService.Graphics.SpriteScreen
        };

        _bouncyChest.Click += BouncyChest_Click;
        _bouncyChest.RightMouseButtonPressed += BouncyChest_Click;
        Service.ResetWatcher.DailyReset += ResetOccurred;

        return Task.CompletedTask;

    }

    private void BouncyChest_Click(object sender, MouseEventArgs e)
    {
        InputHelper.DoHotKey(Service.Settings.WizardsVaultKeybind.Value);
        _bouncyChest.ChestOpen = true;
    }


    private void Init()
    {
        var last = Service.Persistance.GetLastSave(Service.CurrentAccountName);
        if (last < Service.ResetWatcher.LastDailyReset)
        {
            ResetOccurred(this, last);
        }
        else
        {
            HandleHide(false);
        }
    }

    private void ResetOccurred(object sender, DateTime reset)
    {
        Debug.WriteLine("------ HAPPY RESET -------"+reset.ToString());
        _bouncyChest.ChestOpen = false;
        _bouncyChest.Show();

    }
    protected void HandleHide(bool save=true)
    {
        if(save) Service.Persistance.SaveClear(Service.CurrentAccountName);
        _bouncyChest.ChestOpen = true;
    }

    public void ChestClickHandler(object sender, MouseEventArgs e)
    {
        InputHelper.DoHotKey(new KeyBinding(ModifierKeys.Shift, Keys.H));
        HandleHide();
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

        try
        {
            _bouncyChest.Click -= BouncyChest_Click;
            _bouncyChest.RightMouseButtonPressed -= BouncyChest_Click;
            _bouncyChest.Dispose();
            Service.ResetWatcher.DailyReset -= ResetOccurred;
        }
        catch (Exception e)
        {

        }

        Service.ContentsManager?.Dispose();
        Service.Textures?.Dispose();
        Service.ResetWatcher?.Dispose();
    }

    protected override void Update(GameTime gameTime)
    {
        Service.ResetWatcher?.Update(gameTime);
    }

    private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
    {
        Task.Run(async () =>
        {
            Service.CurrentAccountName = await AccountNameService.UpdateAccountName();
            Init();
        });
    }

}