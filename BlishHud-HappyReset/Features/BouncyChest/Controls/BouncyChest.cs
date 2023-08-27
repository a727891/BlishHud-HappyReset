using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Glide;
using HappyReset.Features.Shared.Controls;
using HappyReset.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HappyReset.Features.BouncyChest.Controls;

public class BouncyChest : Control
{
    protected override CaptureType CapturesInput() => CaptureType.Mouse;

    protected Image chestImage;

    protected bool _visibleOnScreen = true;

    private const int BOUNCE_COUNT = 15;
    private const float BOUNCE_DURATION = 1f;
    private const float BOUNCE_DELAY = 1.4f;

    private const float BOUNCE_ROTATION = -MathHelper.PiOver4 / 4;

    private readonly Texture2D _shineTexture = GameService.Content.GetTexture("controls/bouncynotification/965696");

    private AsyncTexture2D _chestTexture;
    public AsyncTexture2D ChestTexture
    {
        get => _chestTexture;
        set => SetProperty(ref _chestTexture, value);
    }

    private AsyncTexture2D _openChestTexture;
    public AsyncTexture2D OpenChestTexture
    {
        get => _openChestTexture;
        set => SetProperty(ref _openChestTexture, value);
    }

    private bool _chestOpen = false;
    public bool ChestOpen
    {
        get => _chestOpen;
        set => SetProperty(ref _chestOpen, value);
    }

    private int _wiggleDirection = 1;
    private bool _nonOpp = false;
    private float _rotation = 0f;

    private bool _shouldWiggle = false;

    public BouncyChest()
    {
        Parent = GameService.Graphics.SpriteScreen;

        ChestTexture = Service.Textures!.DailyChestIcon;

        

        Size = new Point(ChestTexture.Width, ChestTexture.Height);

        
        Service.ResetWatcher.DailyReset += ResetOccurred;
        _shouldWiggle = Service.Settings.WiggleChest.Value == Settings.Enums.BounceState.Bouncy;
        Service.Settings.WiggleChest.SettingChanged += WiggleChest_SettingChanged;

        LeftMouseButtonPressed += ChestClickHandler;
        RightMouseButtonPressed += ChestClickHandler;

        AlignWithMinimap();

        Init();

        DoWiggle();

    }

    private void WiggleChest_SettingChanged(object sender, ValueChangedEventArgs<Settings.Enums.BounceState> e)
    {
        _shouldWiggle = e.NewValue == Settings.Enums.BounceState.Bouncy;
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

    public void ChestClickHandler(object sender, MouseEventArgs e)
    {
        InputHelper.DoHotKey(new KeyBinding(ModifierKeys.Shift, Keys.H));
        HandleHide();
    }
    private async void DoWiggle()
    {
        if (!_shouldWiggle) return;
        _nonOpp = !_nonOpp;
        _wiggleDirection = 1;
        await Task.Delay(TimeSpan.FromSeconds(BOUNCE_DELAY));
        try
        {
            GameService.Animation.Tweener.Tween(this, new { _rotation = BOUNCE_ROTATION }, BOUNCE_DURATION / BOUNCE_COUNT)
                .Reflect()
                .Repeat(BOUNCE_COUNT)
                .Ease(Ease.BounceInOut)
                .Rotation(Tween.RotationUnit.Radians)
                // Almost certainly a better way to do this if I thought about it for a bit longer
                .OnRepeat(() => _wiggleDirection *= (_nonOpp = !_nonOpp) ? -1 : 1)
                .OnComplete(DoWiggle);
        }catch(Exception e)
        {

        }
    }

/*    public override void DoUpdate(GameTime gameTime)
    {
        *//*this.Location = new Point(GameService.Graphics.SpriteScreen.Width - this.Width - 24  Distance from right edge , (GameService.Gw2Mumble.UI.IsCompassTopRight
                                                                                                  COMPASS TOP    RIGHT ? GameService.Graphics.SpriteScreen.Height - 24  Distance from bottom edge
                                                                                                  COMPASS BOTTOM RIGHT: GameService.Graphics.SpriteScreen.Height - 35  Distance from bottom edge of map - (int)(GameService.Gw2Mumble.UI.CompassSize.Height / 0.897f))
                                                                                                                          - this.Height - 64 *//* Above actual bouncy chests *//* - 12 *//* Buffer from other bouncy chests *//*);
        *//*
        AlignWithMinimap();
    }*/

    protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
    {
        if (!GameService.GameIntegration.Gw2Instance.IsInGame)
        {
            // Only show when we're in game. This is a deliberate form over function choice.
            return;
        }

        //spriteBatch.DrawOnCtrl(this, _shineTexture, bounds.ScaleBy(1.5f).OffsetBy(bounds.Width / 2, bounds.Height / 2), null, Color.White * 0.8f, (float)GameService.Overlay.CurrentGameTime.TotalGameTime.TotalSeconds * -1.3f, _shineTexture.Bounds.Size.ToVector2() / 2);
        //spriteBatch.DrawOnCtrl(this, this.MouseOver || this.ChestOpen ? this.OpenChestTexture : this.ChestTexture, bounds.OffsetBy(bounds.Width / 2, bounds.Height / 2), null, Color.White, this.ChestOpen ? 0 : _rotation * _wiggleDirection, this.ChestTexture.Texture.Bounds.Size.ToVector2() / 2);
        spriteBatch.DrawOnCtrl(this, ChestTexture, new(0, 0, 100, 100));
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
