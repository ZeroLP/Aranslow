using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Aranslow.Tools.Debugging
{
    internal class GameEngineMetrics
    {
        private static SpriteFont MetricsFont;

        private class MetricsProperties
        {
            internal class FrameCounter
            {
                internal static float FramesPerSecond = 0f;
                internal static float TotalTimeElapsed;
                internal static float DisplayFPS;
            }
        }

        static GameEngineMetrics()
        {
            MetricsFont = Engine.CTNManager.Load<SpriteFont>("Sprites/Font/Default");
        }

        internal static bool IsDrawRendererMetrics = false;
        internal static void DrawRendererMetrics(GraphicsDevice gDevice, SpriteBatch batch)
        {
            if (IsDrawRendererMetrics)
            {
                batch.Begin();
                batch.DrawString(MetricsFont, $"Drawcalls: {gDevice.Metrics.DrawCount}" +
                                              $"\nSprites: {gDevice.Metrics.SpriteCount}" +
                                              $"\nTextures: {gDevice.Metrics.TextureCount}", new Vector2(10f, 25f), Color.Cyan);

                batch.End();
            }
        }

        internal static void DrawFramesPerSecond(GameTime gTime, SpriteBatch batch)
        {
            float elapsed = (float)gTime.ElapsedGameTime.TotalMilliseconds;
            MetricsProperties.FrameCounter.TotalTimeElapsed += elapsed;

            if (MetricsProperties.FrameCounter.TotalTimeElapsed >= 1000)
            {
                MetricsProperties.FrameCounter.DisplayFPS = MetricsProperties.FrameCounter.FramesPerSecond;
                MetricsProperties.FrameCounter.FramesPerSecond = 0;
                MetricsProperties.FrameCounter.TotalTimeElapsed = 0;
            }

            MetricsProperties.FrameCounter.FramesPerSecond++;

            batch.DrawString(MetricsFont, $"FPS: {MetricsProperties.FrameCounter.DisplayFPS}", new Vector2(10f, 10f), Color.Yellow);
        }
    }
}
