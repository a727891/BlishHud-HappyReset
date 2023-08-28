using System;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace HappyReset.Services;

public class TextureService : IDisposable
{
    public TextureService(ContentsManager contentsManager)
    {
        DailyChestIcon = contentsManager.GetTexture(@"925822.png");
        DailyChestOpenedIcon = contentsManager.GetTexture(@"925823.png");
        ShineTexture = contentsManager.GetTexture(@"965696.png");
    }

    public void Dispose()
    {
        DailyChestIcon.Dispose();
        DailyChestOpenedIcon.Dispose();
        ShineTexture.Dispose();
    }

    public Texture2D DailyChestIcon { get; }//925822
    public Texture2D DailyChestOpenedIcon { get; }//925823
    public Texture2D ShineTexture { get; }//965696


}