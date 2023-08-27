using System;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace HappyReset.Features.Shared.Services;

public class TextureService : IDisposable
{
    public TextureService(ContentsManager contentsManager)
    {

       

        DailyChestIcon = contentsManager.GetTexture(@"Login_rewards_chest.png");
        

    }

    public void Dispose()
    {

        DailyChestIcon.Dispose();

    }
  
    public Texture2D DailyChestIcon { get; }


}