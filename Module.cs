using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using HappyReset.Settings;
using HappyReset.Services;
using Blish_HUD.GameIntegration;
using Gw2Sharp.WebApi.V2.Models;
using System.Collections.Generic;
using Blish_HUD.Input;
using HappyReset.Utils;
using System;
using System.Diagnostics;
using HappyReset.Controls;

namespace HappyReset;


[Export(typeof(Blish_HUD.Modules.Module))]
public class Module : Blish_HUD.Modules.Module
{
    public static string DIRECTORY_PATH = "happyReset"; //Defined folder in manifest.json

    internal static readonly Logger ModuleLogger = Logger.GetLogger<Module>();

    private BouncyNotification? _bouncyChest;

    [ImportingConstructor]
    public Module([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters)
    {
        Service.ModuleInstance = this;
        Service.ContentsManager = moduleParameters.ContentsManager;
        Service.Gw2ApiManager = moduleParameters.Gw2ApiManager;
        Service.DirectoriesManager = moduleParameters.DirectoriesManager;
    }

    protected override void DefineSettings(SettingCollection settings) => Service.Settings = new SettingService(settings);

    public override IView GetSettingsView() => new Settings.ModuleMainSettingsView();

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
        Service.ResetWatcher.DailyReset += ResetOccurred;

        if (Service.Gw2ApiManager.HasPermissions(AccountNameService.NecessaryApiTokenPermissions))
        {
            UpdateAccountName();
        }
       
        return Task.CompletedTask;

    }

    private void Init()
    {
        var lastLoggedReset = Service.Persistance.GetLastSave(Service.CurrentAccountName);
        if (lastLoggedReset < Service.ResetWatcher.LastDailyReset)
        {
            ResetOccurred(this, lastLoggedReset);
        }
        else
        {
            HandleHide(false);
        }
    }

    private void ResetOccurred(object sender, DateTime reset)
    {
        CreateTheBouncyChest();

    }
    protected void HandleHide(bool save = true)
    {
        if (save) Service.Persistance.SaveClear(Service.CurrentAccountName);
        DestroyTheBouncyChest();
    }

    private void CreateTheBouncyChest()
    {
        if(_bouncyChest != null) { 
            return; 
        }

        _bouncyChest = new BouncyNotification(
           Service.Textures!.DailyChestIcon,
           Service.Textures!.DailyChestOpenedIcon
        )
        {
            Parent = GameService.Graphics.SpriteScreen
        };

        _bouncyChest.Click += BouncyChest_Click;
        _bouncyChest.RightMouseButtonPressed += BouncyChest_Click;

    }

    private void DestroyTheBouncyChest()
    {
        if(_bouncyChest == null ) {
            return;
        }

        _bouncyChest.Click -= BouncyChest_Click;
        _bouncyChest.RightMouseButtonPressed -= BouncyChest_Click;
        _bouncyChest.Dispose();
        _bouncyChest = null;
    }

    private void BouncyChest_Click(object sender, MouseEventArgs e)
    {
        InputHelper.DoHotKey(Service.Settings.WizardsVaultKeybind.Value);
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
        try
        {
            Service.Gw2ApiManager.SubtokenUpdated -= Gw2ApiManager_SubtokenUpdated;
            Service.ResetWatcher.DailyReset -= ResetOccurred;

            DestroyTheBouncyChest();

            Service.ContentsManager?.Dispose();
            Service.Textures?.Dispose();
            Service.ResetWatcher?.Dispose();

        }
        catch (Exception e)
        {
            ModuleLogger.Warn("HappyReset threw exception in Unload, " + e.Message);
        }   
    }

    protected override void Update(GameTime gameTime)
    {
        _bouncyChest?.Update(gameTime);
        Service.ResetWatcher?.Update(gameTime);
    }

    private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
    {
        UpdateAccountName();
    }

    private void UpdateAccountName()
    {
        Task.Run(async () =>
        {
            Service.CurrentAccountName = await AccountNameService.UpdateAccountName();
            Debug.WriteLine("New API token, accountName = " + Service.CurrentAccountName);
            Init();
        });
    }

}