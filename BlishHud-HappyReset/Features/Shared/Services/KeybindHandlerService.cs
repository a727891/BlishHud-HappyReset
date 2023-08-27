using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;


namespace HappyReset.Features.Shared.Services;

public class KeyBindHandlerService : IDisposable
{
    public KeyBindHandlerService(
        SettingEntry<KeyBinding> keyBindSetting,
        SettingEntry<bool> toggleControlSetting
    )
    {
        _keyBindSetting          = keyBindSetting;
        _toggleControlSetting    = toggleControlSetting;

        _keyBindSetting.Value.Enabled = true;
        _keyBindSetting.Value.Activated += OnKeyBindActivated;

    }

    public void Dispose()
    {
        _keyBindSetting.Value.Enabled = false;
        _keyBindSetting.Value.Activated -= OnKeyBindActivated;
    }

    private void OnKeyBindActivated(object sender, EventArgs e)
    {
        _toggleControlSetting.Value = !_toggleControlSetting.Value;
    }

    private readonly SettingEntry<KeyBinding> _keyBindSetting;
    private readonly SettingEntry<bool> _toggleControlSetting;
}