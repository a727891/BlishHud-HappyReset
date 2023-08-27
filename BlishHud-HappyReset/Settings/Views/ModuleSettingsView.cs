using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using HappyReset.Utils;

namespace HappyReset.Settings.Views;

public class ModuleMainSettingsView: View
{
    protected override void Build(Container buildPanel)
    {
        var panel = new FlowPanel();

        panel.BeginFlow(buildPanel)
            .AddString("Happy Reset - Settings")
            .AddSpace()
            .AddString("Keybind - Make sure this matches your in-game settings")
            .AddSetting(Service.Settings.WizardsVaultKeybind)
            .AddSpace()
            .AddString("Options")
            .AddSettingEnum(Service.Settings.WiggleChest)
            .AddSettingEnum(Service.Settings.ShouldShine);
            

        
    }
}
