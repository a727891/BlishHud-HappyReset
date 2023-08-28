
using Microsoft.Xna.Framework;
using static Blish_HUD.GameService;


namespace HappyReset.Utils
{
    public static class CompassData
    {
        private const double BlishScale = 1 / .897;

        private static int _lastTick;

        //private static Vector2 _mapCenter;
        //public static Vector2 MapCenter => UpdateAndReturn(ref _mapCenter);

        private static Matrix _mapRotation;
        public static Matrix MapRotation => UpdateAndReturn(ref _mapRotation);

        private static float _scale;
        public static float Scale => UpdateAndReturn(ref _scale);

        private static Rectangle _screenBounds;
        public static Rectangle ScreenBounds => UpdateAndReturn(ref _screenBounds);

        private static Vector2 _boundsCenter;
        public static Vector2 BoundsCenter => UpdateAndReturn(ref _boundsCenter);

        private static T UpdateAndReturn<T>(ref T value)
        {
            if (Gw2Mumble.Tick != _lastTick)
            {
                _lastTick = Gw2Mumble.Tick;

                //_mapCenter = Gw2Mumble.UI.MapCenter.ToXnaVector2();
                _mapRotation = Matrix.CreateRotationZ(
                    Gw2Mumble.UI.IsCompassRotationEnabled && !Gw2Mumble.UI.IsMapOpen
                        ? (float)Gw2Mumble.UI.CompassRotation
                        : 0);

                _screenBounds = MumbleUtils.GetMapBounds();
                _scale = (float)(BlishScale / Gw2Mumble.UI.MapScale);
                _boundsCenter = _screenBounds.Location.ToVector2() + _screenBounds.Size.ToVector2() / 2f;
            }

            return value;
        }
    }
}
