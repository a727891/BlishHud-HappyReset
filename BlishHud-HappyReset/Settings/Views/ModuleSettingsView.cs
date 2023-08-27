using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using HappyReset.Utils;

namespace HappyReset.Settings.Views;

public class ModuleMainSettingsView: View
{
    protected override void Build(Container buildPanel)
    {
        buildPanel.AddControl(new StandardButton 
        {
            Parent = buildPanel,
            Text = "HELLO",
            Size = buildPanel.Size.Scale(0.20f),
            Location = buildPanel.Size.Half() - buildPanel.Size.Scale(0.20f).Half(),
            
        }).Click += (_, _) => { };
    }
}
