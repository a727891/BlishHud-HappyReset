using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using HappyReset.Features.Shared.Controls;
using HappyReset.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace HappyReset.Features.BouncyChest.Controls;

public class BouncyChest : OnScreenControl
{
    protected override CaptureType CapturesInput() => CaptureType.Mouse;

    protected Image chestImage;

    protected bool _visibleOnScreen = true;

    public BouncyChest() : base(GameService.Graphics.SpriteScreen)
    {

        var texture = Service.Textures!.DailyChestIcon;

        Size = new Point(texture.Width, texture.Height);

        chestImage = new Image()
        {
            Texture = texture,
            Parent = this,
            Location = new(0, 0),
            Size = this.Size

        };

        Service.ResetWatcher.DailyReset += ResetOccurred;


        chestImage.LeftMouseButtonPressed += ChestClickHandler;
        chestImage.RightMouseButtonPressed += ChestClickHandler;

        AlignWithMinimap();

        Init();

    }

    public void AccountNameChanged()
    {
        Init();
    }

    private void Init()
    {
        var last = Service.Persistance.GetLastSave(Service.CurrentAccountName);
        if( last < Service.ResetWatcher.LastDailyReset)
        {
            ResetOccurred(this, last);
        }
        else
        {
            _visibleOnScreen = false;
        }
    }

    private void ResetOccurred(object sender, DateTime reset)
    {
        //Debug.WriteLine("------ HAPPY RESET -------");
        _visibleOnScreen = true;
    }

    protected void HandleHide()
    {
        Service.Persistance.SaveClear(Service.CurrentAccountName);
        _visibleOnScreen= false;
    }

    protected void ChestClickHandler(object sender, MouseEventArgs e)
    {
        InputHelper.DoHotKey(new KeyBinding(ModifierKeys.Shift, Keys.H));
        HandleHide();
    }


    protected override void DisposeControl()
    {
        Service.ResetWatcher.DailyReset -= ResetOccurred;
        chestImage.LeftMouseButtonPressed -= ChestClickHandler;
        chestImage.RightMouseButtonPressed -= ChestClickHandler;
        chestImage.Dispose();

        base.DisposeControl();
    }




    public void Update()
    {
        var shouldBeVisible =
          _visibleOnScreen &&
          GameService.GameIntegration.Gw2Instance.Gw2IsRunning &&
          GameService.GameIntegration.Gw2Instance.IsInGame &&
          GameService.Gw2Mumble.IsAvailable &&
          !GameService.Gw2Mumble.UI.IsMapOpen;

        if (!Visible && shouldBeVisible)
            Show();
        else if (Visible && !shouldBeVisible)
            Hide();

        if(Location.X < 0 || Location.Y < 0)
        {
            AlignWithMinimap();
        }
    }

    protected void AlignWithMinimap()
    {
        var IsCompassTopRight = Gw2MumbleService.Gw2Mumble.UI.IsCompassTopRight;
        var spriteScreenSize = GameService.Graphics.SpriteScreen.Size;

        var compassMapBounds = CompassData.ScreenBounds;


        if (!IsCompassTopRight)
        {//Bottom right map - place chest above map
            Location = new Point(compassMapBounds.X, compassMapBounds.Y) - new Point(0,this.Size.Y);
        }
        else
        {//top right map - place chest on bottom of spriteScreenSize
            Location = spriteScreenSize  - this.Size;
        }

    }

}
