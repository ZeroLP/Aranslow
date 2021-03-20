using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Aranslow.Tools;

namespace Aranslow.GameObjects
{
    /// <summary>
    /// LocalPlayer class
    /// </summary>
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
            public static ASSprite BasicAttackAnimationSprite;
        }

        private WalkState CurrentWalkState;

        public override void LoadContent()
        {
            AnimationSprite.IdleAnimationSprite = new ASSprite(Engine.CTNManager.Load<Texture2D>("Sprites/Player/Idle"), 0.2f, true);
            AnimationSprite.WalkAnimationSprite = new ASSprite(Engine.CTNManager.Load<Texture2D>("Sprites/Player/Walk"), 0.1f, true);
            AnimationSprite.BasicAttackAnimationSprite = new ASSprite(Engine.CTNManager.Load<Texture2D>("Sprites/Player/BasicAttack"), 0.05f, false);

            base.LoadContent();
        }

        public override void Update()
        {
            if (CurrentActionState == ActionState.Idle)
                this.Sprite.PlayAnimation(AnimationSprite.IdleAnimationSprite);
            else if (CurrentActionState == ActionState.Walk)
                this.Sprite.PlayAnimation(AnimationSprite.WalkAnimationSprite);
            else if (CurrentActionState == ActionState.BasicAttack)
                this.Sprite.PlayAnimation(AnimationSprite.BasicAttackAnimationSprite);

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

        private SpellBook SpellBookHandle;
        public SpellBook GetSpellBook()
        {
            if (SpellBookHandle == null)
                return SpellBookHandle = new SpellBook(this);
            else
                return SpellBookHandle;
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

        public class SpellBook
        {
            private ASLocalClient LocalPlayer;
            public SpellBook(ASLocalClient lPlayer)
            {
                LocalPlayer = lPlayer;
            }

            public ASSpellClient CurrrentCastingSpell;
            public ASSpellClient LastCastedSpell;

            /// <summary>
            /// Casts basic attack.
            /// </summary>
            public void CastBasicAttack()
            {
                if(CurrrentCastingSpell != null && CurrrentCastingSpell.Slot == ASSpellClient.SpellSlot.BasicAttack)
                {
                    if (Engine.SecondaryGameTimeHandle.TotalGameTime.TotalMilliseconds < CurrrentCastingSpell.EndTime)
                        Logger.Log($"LocalPlayer failed to cast basic attack due to possible override");
                    else
                    {
                        LastCastedSpell = CurrrentCastingSpell;
                        ObjectManager.DeleteObject(CurrrentCastingSpell);
                        CurrrentCastingSpell = null;
                    }
                }
                else if(CurrrentCastingSpell == null)
                {
                    var createdSpell = new ASSpellClient(new Vector2(LocalPlayer.WorldPosition.X + 5, LocalPlayer.WorldPosition.Y));

                    if (createdSpell != null)
                    {
                        Logger.Log("LocalPlayer has casted basic attack");

                        LocalPlayer.CurrentActionState = ActionState.BasicAttack;

                        createdSpell.Slot = ASSpellClient.SpellSlot.BasicAttack;
                        createdSpell.BoundingRadius = 50;
                        createdSpell.EndTime = Engine.SecondaryGameTimeHandle.TotalGameTime.TotalMilliseconds + 3000;

                        CurrrentCastingSpell = createdSpell;
                        ObjectManager.AddObject(createdSpell);
                    }
                    else
                        Logger.Log("Failed to create BasicAttack spell for LocalPlayer");
                }
            }
        }

        public enum WalkState
        {
            Right, 
            Left
        }
    }
}
