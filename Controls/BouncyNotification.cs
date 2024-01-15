using Blish_HUD.Content;
using Blish_HUD;
using Glide;
using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;
using HappyReset.Utils;
using HappyReset.Enum;

namespace HappyReset.Controls;

internal class BouncyNotification : Control
{

    // Consider pivoting this to a common control with support for showing multiple.

    private const int BOUNCE_COUNT = 15;
    private const float BOUNCE_DURATION = 1f;
    private const float BOUNCE_DELAY = 1.4f;

    private const float BOUNCE_ROTATION = -MathHelper.PiOver4 / 4;

    private readonly Texture2D _shineTexture = Service.Textures!.ShineTexture;

    private bool _shouldBounce = true;
    private bool _shouldShine = true;
    private ChestPosition _chestLocation = ChestPosition.MINIMAP_TOP_LEFT;

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

    private bool _isAnimating = false;
    private int _wiggleDirection = 1;
    private bool _nonOpp = false;
    private float _rotation = 0f;

    public BouncyNotification(
        AsyncTexture2D chestTexture,
        AsyncTexture2D openChestTexture)
    {
        _chestTexture = chestTexture;
        _openChestTexture = openChestTexture;

        _chestOpen = false;

        Size = new Point(64, 64);
        ClipsBounds = false;

        _chestLocation = Service.Settings.ChestLocation.Value;
        _shouldBounce = Service.Settings.WiggleChest.Value;
        _shouldShine = Service.Settings.ShouldShine.Value;
        Service.Settings.ChestLocation.SettingChanged += ChestLocation_SettingChanged;
        Service.Settings.WiggleChest.SettingChanged += WiggleChest_SettingChanged;
        Service.Settings.ShouldShine.SettingChanged += ShouldShine_SettingChanged;

        DoWiggle();
    }

    private void ChestLocation_SettingChanged(object sender, ValueChangedEventArgs<Enum.ChestPosition> e)
    {
        _chestLocation = e.NewValue;
    }

    private void ShouldShine_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
    {
        _shouldShine = e.NewValue;
    }

    private void WiggleChest_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
    {
        _shouldBounce = e.NewValue;

        DoWiggle();
    }

    private async void DoWiggle()
    {
        if (!_shouldBounce || _isAnimating) return;
        _isAnimating = true;
        _nonOpp = !_nonOpp;
        _wiggleDirection = 1;
        await Task.Delay(TimeSpan.FromSeconds(BOUNCE_DELAY));
        GameService.Animation.Tweener.Tween(this, new { _rotation = BOUNCE_ROTATION }, BOUNCE_DURATION / BOUNCE_COUNT)
            .Reflect()
            .Repeat(BOUNCE_COUNT)
            .Ease(Ease.BounceInOut)
            .Rotation(Tween.RotationUnit.Radians)
            // Almost certainly a better way to do this if I thought about it for a bit longer
            .OnRepeat(() => _wiggleDirection *= (_nonOpp = !_nonOpp) ? -1 : 1)
            .OnComplete(RestartWiggle);
    }
    private void RestartWiggle()
    {
        _isAnimating = false;
        if (ChestOpen) return;
        DoWiggle();

    }

    public override void DoUpdate(GameTime gameTime)
    {
        if (ChestOpen) return;

        var shouldBeVisible =
         !_chestOpen &&
         GameService.GameIntegration.Gw2Instance.Gw2IsRunning &&
         GameService.GameIntegration.Gw2Instance.IsInGame &&
         GameService.Gw2Mumble.IsAvailable &&
         !GameService.Gw2Mumble.UI.IsMapOpen;

        if (!Visible && shouldBeVisible)
            Show();
        else if (Visible && !shouldBeVisible)
            Hide();


        if (!_chestOpen && Visible)
        {
            AlignWithMinimap();
        }
    }


    protected void AlignWithMinimap()
    {
        var IsCompassTopRight = GameService.Gw2Mumble.UI.IsCompassTopRight;
        var spriteScreenSize = GameService.Graphics.SpriteScreen.Size;

        var compassMapBounds = CompassData.ScreenBounds;

        switch (_chestLocation)
        {
            case ChestPosition.MINIMAP_TOP_LEFT:
                //Bottom right map - place chest above map
                Location = new Point(compassMapBounds.X, compassMapBounds.Y) - new Point(0, Size.Y);
                break;
            case ChestPosition.MINIMAP_TOP_RIGHT:

                Location = new Point(compassMapBounds.X+compassMapBounds.Width - 20, compassMapBounds.Y ) - Size;
                break;
            case ChestPosition.SCREEN_BOTTOM_RIGHT:
                //top right map - place chest on bottom of spriteScreenSize
                Location = spriteScreenSize - Size - new Point(20, 20);
                break;
            case ChestPosition.MINIMAP_INSIDE_TOP_LEFT:
                Location = new Point(compassMapBounds.X , compassMapBounds.Y);
                break;
            case ChestPosition.MINIMAP_INSIDE_TOP_RIGHT:
                Location = new Point(compassMapBounds.X+compassMapBounds.Width, compassMapBounds.Y) - new Point(Size.X, 0);
                break;
            case ChestPosition.MINIMAP_INSIDE_BOTTOM_LEFT:
                Location = new Point(compassMapBounds.X, compassMapBounds.Y + compassMapBounds.Height) - new Point(0, Size.Y);
                break;
            case ChestPosition.MINIMAP_INSIDE_BOTTOM_RIGHT:
                Location = new Point(compassMapBounds.X + compassMapBounds.Width, compassMapBounds.Y + compassMapBounds.Height) - Size;
                break;
        }

        /* if (!IsCompassTopRight)
         {//Bottom right map - place chest above map
             Location = new Point(compassMapBounds.X, compassMapBounds.Y) - new Point(0, Size.Y);
         }
         else
         {
         //top right map - place chest on bottom of spriteScreenSize
                 Location = spriteScreenSize - Size - new Point(20, 20);
         }*/
    }


    protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
    {
        if (ChestOpen)
        {
            return;
        }
        if (!GameService.GameIntegration.Gw2Instance.IsInGame)
        {
            // Only show when we're in game. This is a deliberate form over function choice.
            return;
        }
        if (_shouldShine)
        {
            spriteBatch.DrawOnCtrl(
                this,
                _shineTexture,
                bounds.ScaleBy(1.5f).OffsetBy(bounds.Width / 2, bounds.Height / 2),
                null,
                Color.White * 0.8f,
                (float)GameService.Overlay.CurrentGameTime.TotalGameTime.TotalSeconds * -1.3f,
                _shineTexture.Bounds.Size.ToVector2() / 2
            );
        }
        spriteBatch.DrawOnCtrl(
            this,
            MouseOver || ChestOpen ? OpenChestTexture : ChestTexture,
            bounds.OffsetBy(bounds.Width / 2, bounds.Height / 2),
            null,
            Color.White,
            MouseOver || ChestOpen ? 0 : _rotation * _wiggleDirection,
            ChestTexture.Texture.Bounds.Size.ToVector2() / 2
        );
    }

}
