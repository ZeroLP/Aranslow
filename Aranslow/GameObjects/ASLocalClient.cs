using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aranslow.GameObjects
{
    public class ASLocalClient : ASBaseClient
    {
        public ASLocalClient(Vector2 wPosition) : base(wPosition)
        {
            this.Health = 100;
            MotionSpeed = 150;
            BoundingRadius = 80;
        }

        private class AnimationSprite
        {
            public static ASSprite IdleAnimationSprite;
            public static ASSprite WalkAnimationSprite;
            public static ASSprite JumpAnimationSprite;
        }

        private WalkState CurrentWalkState;

        public override void LoadContent()
        {
            AnimationSprite.IdleAnimationSprite = new ASSprite(Engine.CTNManager.Load<Texture2D>("Sprites/Player/Idle"), 0.2f, true);
            AnimationSprite.WalkAnimationSprite = new ASSprite(Engine.CTNManager.Load<Texture2D>("Sprites/Player/Walk"), 0.1f, true);
            //AnimationSprite.JumpAnimationSprite = new ASSprite(Engine.CTNManager.Load<Texture2D>("Sprites/Player/Jump"), 0.5f, true);

            base.LoadContent();
        }

        public override void Update()
        {
            if (CurrentActionState == ActionState.Idle)
                this.Sprite.PlayAnimation(AnimationSprite.IdleAnimationSprite);
            else if (CurrentActionState == ActionState.Walk)
                this.Sprite.PlayAnimation(AnimationSprite.WalkAnimationSprite);
            else if (CurrentActionState == ActionState.Jump)
                this.Sprite.PlayAnimation(AnimationSprite.JumpAnimationSprite);

            if (CurrentWalkState == WalkState.Right)
                this.CharSpriteInvertState = SpriteEffects.None;
            else if (CurrentWalkState == WalkState.Left)
                this.CharSpriteInvertState = SpriteEffects.FlipHorizontally;

            base.Update();
        }

        public override void Draw(GameTime gtime, SpriteBatch sBatch)
        {
            if (this.IsAlive)
                Sprite.Draw(gtime, sBatch, WorldPosition, this.CharSpriteInvertState);

            base.Draw(gtime, sBatch);
        }

        public void Stand() => CurrentActionState = ActionState.Idle;

        public void Walk(WalkState wState)
        {
            if (wState == WalkState.Right)
                this.WorldPosition.X += this.MotionSpeed * (float)Engine.SecondaryGameTimeHandle.ElapsedGameTime.TotalSeconds;
            else
                this.WorldPosition.X -= this.MotionSpeed * (float)Engine.SecondaryGameTimeHandle.ElapsedGameTime.TotalSeconds;

            CheckBoundsAndCalibratePosition();

            CurrentActionState = ActionState.Walk;
            CurrentWalkState = wState;
        }

        private void CheckBoundsAndCalibratePosition()
        {
            if (this.BoundingBox.Left <= 0)
                this.WorldPosition.X = BoundingBox.Width / 2;
            else if (this.BoundingBox.Right > Engine.RenderDeviceManager.PreferredBackBufferWidth)
                this.WorldPosition.X = Engine.RenderDeviceManager.PreferredBackBufferWidth - (BoundingBox.Width / 2) - 1;
        }

        public enum WalkState
        {
            Right, 
            Left
        }
    }
}
