using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Input;


namespace HappyReset.Settings;

public class SettingService // singular because Setting"s"Service already exists in Blish
{
    public SettingEntry<KeyBinding> WizardsVaultKeybind { get; }
    public SettingEntry<bool> WiggleChest { get; }
    public SettingEntry<bool> ShouldShine { get; }

    public SettingService(SettingCollection settings)
    {
        WizardsVaultKeybind = settings.DefineSetting("HR_WizVault_Keybind",
            new KeyBinding(ModifierKeys.Shift, Keys.H),
        () => "Wizard's Vault Keybind",
        () => "This must match your in-game Wizard's Vault keybind (F11 -> Keybinds)");
        //WizardsVaultKeybind.Value.Enabled = true;

        WiggleChest = settings.DefineSetting("HR_Wiggle",
            true,
        () => "Wiggle and bounce",
        () => "The chest wiggles like the old daily chest");


        ShouldShine = settings.DefineSetting("HR_Shine",
            true,
        () => "Shiny background",
        () => "The chest shines like the old daily chest");

    }

}