using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Input;

namespace HappyReset.Settings.Services;

public class SettingService // singular because Setting"s"Service already exists in Blish
{
    public SettingEntry<KeyBinding> WizardsVaultKeybind { get; }

    public SettingService(SettingCollection settings)
    {
       

        WizardsVaultKeybind = settings.DefineSetting("HR_WizVault_Keybind",
            new KeyBinding(ModifierKeys.Shift,Keys.H),
        () => "keybind label",
        () => "keybind tooltip");
        WizardsVaultKeybind.Value.Enabled = true;

    }

}