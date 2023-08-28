using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Input;
using HappyReset.Settings.Enums;


namespace HappyReset.Settings.Services;

public class SettingService // singular because Setting"s"Service already exists in Blish
{
    public SettingEntry<KeyBinding> WizardsVaultKeybind { get; }
    public SettingEntry<BinaryOption> WiggleChest { get; }
    public SettingEntry<BinaryOption> ShouldShine { get; }

    public SettingService(SettingCollection settings)
    {
       

        WizardsVaultKeybind = settings.DefineSetting("HR_WizVault_Keybind",
            new KeyBinding(ModifierKeys.Shift,Keys.H),
        () => "Wizard's Vault Keybind",
        () => "This must match your in-game Wizard's Vault keybind (F11 -> Keybinds)");
        //WizardsVaultKeybind.Value.Enabled = true;

        WiggleChest = settings.DefineSetting("HR_Wiggle",
            BinaryOption.Yes,
        () => "Wiggle and bounce?",
        () => "Yes - the chest wiggles like the old daily chest");


        ShouldShine = settings.DefineSetting("HR_Shine",
            BinaryOption.Yes,
        () => "Shiny background?",
        () => "Yes - the chest shines like the old daily chest");



    }

}