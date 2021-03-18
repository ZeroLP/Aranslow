using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Aranslow.Tools;
using Aranslow.Tools.Debugging;

namespace Aranslow
{
    public class Engine : Game
    {
        public static GameTime SecondaryGameTimeHandle;
        public static GraphicsDeviceManager RenderDeviceManager;
        public static SpriteBatch SBatch;
        public static Microsoft.Xna.Framework.Content.ContentManager CTNManager;

        public class Settings
        {
            public class Rendering
            {
                public static Color RendererResetColour = new Color(50, 59, 79);
                public static RasterizerState RastState = new RasterizerState { MultiSampleAntiAlias = true };
                public static bool IsBoundingBoxRendered = true;
            }
        }

        public Engine()
        {
            RenderDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1134,
                PreferredBackBufferHeight = 550,
                PreferMultiSampling = true
            };

            Content.RootDirectory = "Content"; // Game assets directory
            CTNManager = Content;

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
#if DEBUG
            Logger.CreateLoggerInstance();
#endif

            ObjectManager.PopulateObjects();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SBatch = new SpriteBatch(GraphicsDevice);
            RenderFactory.Initialise(GraphicsDevice);

            foreach (var gObject in ObjectManager.Objects)
                gObject.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            SecondaryGameTimeHandle = gameTime;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            HandleInput(Keyboard.GetState(), Mouse.GetState());

            foreach (var gObject in ObjectManager.Objects)
                gObject.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            RenderDeviceManager.GraphicsDevice.Clear(Settings.Rendering.RendererResetColour);
            SBatch.Begin(rasterizerState: Settings.Rendering.RastState);

            GameEngineMetrics.DrawFramesPerSecond(gameTime, SBatch);

            foreach (var gObject in ObjectManager.Objects)
                gObject.Draw(gameTime, SBatch);

            SBatch.End();

            GameEngineMetrics.DrawRendererMetrics(GraphicsDevice, SBatch);
            base.Draw(gameTime);
        }

        private void HandleInput(KeyboardState currentKeyboardState, MouseState currentMouseState)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Insert)) Settings.Rendering.IsBoundingBoxRendered = true;
            if (currentKeyboardState.IsKeyDown(Keys.Delete)) Settings.Rendering.IsBoundingBoxRendered = false;

            if (currentKeyboardState.IsKeyDown(Keys.Space)) ObjectManager.LocalPlayer.Health = 0;

            if (currentKeyboardState.IsKeyDown(Keys.OemTilde)) GameEngineMetrics.IsDrawRendererMetrics = true;
            else GameEngineMetrics.IsDrawRendererMetrics = false;

            if (currentKeyboardState.IsKeyDown(Keys.Right))
                ObjectManager.LocalPlayer.Walk(GameObjects.ASLocalClient.WalkState.Right);
            else if (currentKeyboardState.IsKeyDown(Keys.Left))
                ObjectManager.LocalPlayer.Walk(GameObjects.ASLocalClient.WalkState.Left);
            else
                ObjectManager.LocalPlayer.Stand();
        }
    }
}
