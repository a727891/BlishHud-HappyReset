using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using HappyReset.Features.Shared.Services;
using HappyReset.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HappyReset.Features.Shared.Controls;

public class OnScreenControl : FlowPanel
{
    private static Vector2 DefaultPadding = new(2, 2);

    protected OnScreenControl(Container parent)
    {

        ControlPadding = DefaultPadding;
        Location = new Point(0,0);
        Size = new Point(0,0);
        Visible = true;
        Parent = parent;


    }

    protected override void DisposeControl()
    {
        base.DisposeControl();
    }


}
