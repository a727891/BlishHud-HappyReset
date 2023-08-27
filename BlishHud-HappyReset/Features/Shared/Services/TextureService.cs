using System;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace HappyReset.Features.Shared.Services;

public class TextureService : IDisposable
{
    public TextureService(ContentsManager contentsManager)
    {

       

        DailyChestIcon = contentsManager.GetTexture(@"Login_rewards_chest.png");
        ShineTexture = contentsManager.GetTexture(@"965696.png");

    }

    public void Dispose()
    {

        DailyChestIcon.Dispose();
        ShineTexture.Dispose();

    }
  
    public Texture2D DailyChestIcon { get; }
    public Texture2D ShineTexture { get; }


}