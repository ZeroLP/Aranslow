using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Aranslow
{
    internal class RenderFactory
    {
        internal static Texture2D LineInstance;
        internal static Texture2D BorderRectangleInstance;

        internal static void Initialise(GraphicsDevice gDevice)
        {
            LineInstance = new Texture2D(gDevice, 1, 1, false, SurfaceFormat.Color);
            LineInstance.SetData<Color>(new Color[] { Color.White });

            BorderRectangleInstance = new Texture2D(Engine.RenderDeviceManager.GraphicsDevice, 1, 1);
            BorderRectangleInstance.SetData(new Color[] { Color.White });

            Tools.Logger.Log($"Initialised RenderFactory");
        }

        internal static void DrawLine(Vector2 lineStart, Vector2 lineEnd, Color lineColour, int thickness = 2)
        {
            double angle = (double)Math.Atan2(lineEnd.Y - lineStart.Y, lineEnd.X - lineStart.X);
            float length = (lineEnd - lineStart).Length();

            Engine.SBatch.Draw(LineInstance, lineStart, null, lineColour, (float)angle, Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0);
        }

        internal static void DrawFont()
        {

        }

        internal static void DrawBox(Rectangle r, Color c, int thickness = 2)
        {
            Engine.SBatch.Draw(BorderRectangleInstance, new Rectangle(r.X, r.Y, thickness, r.Height + thickness), c);
            Engine.SBatch.Draw(BorderRectangleInstance, new Rectangle(r.X, r.Y, r.Width + thickness, thickness), c);
            Engine.SBatch.Draw(BorderRectangleInstance, new Rectangle(r.X + r.Width, r.Y, thickness, r.Height + thickness), c);
            Engine.SBatch.Draw(BorderRectangleInstance, new Rectangle(r.X, r.Y + r.Height, r.Width + thickness, thickness), c);
        }

        internal static void DrawFilledBox(Rectangle r, Color c)
        {
            Texture2D rectTexture = new Texture2D(Engine.RenderDeviceManager.GraphicsDevice, r.Width, r.Height);

            Color[] data = new Color[r.Width * r.Height];

            for (int i = 0; i < data.Length; i++) 
                data[i] = Color.Chocolate;

            rectTexture.SetData(data);

            Engine.SBatch.Draw(rectTexture, new Vector2(r.X, r.Y), c);
        }
    }
}
