using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using HappyReset.Utils;

namespace HappyReset.Settings;

public class ModuleMainSettingsView : View
{
    protected override void Build(Container buildPanel)
    {
        var panel = new FlowPanel();

        panel.BeginFlow(buildPanel)
            .AddSpace()
            .AddString("Keybind - Make sure this matches your in-game settings")
            .AddSetting(Service.Settings.WizardsVaultKeybind)
            .AddSpace()
            .AddString("Display Options")
            .AddSetting(Service.Settings.ChestLocation)
            .AddSetting(Service.Settings.WiggleChest)
            .AddSetting(Service.Settings.ShouldShine);



    }
}
