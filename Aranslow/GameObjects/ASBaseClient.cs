using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Aranslow.GameObjects
{
    /// <summary>
    /// Base class for all game objects.
    /// </summary>
    public abstract class ASBaseClient 
    {
        public ASBaseClient(Vector2 wPosition)
        {
            WorldPosition = wPosition;
        }

        public int ObjectID => ObjectManager.Objects.IndexOf(this);
        public ObjectClientType? ObjectType = ObjectClientType.BaseObject;
        public bool IsAlive => Health >= 1;
        public bool IsSpawned => WorldPosition != Vector2.Zero && IsAlive;

        public ASSpritePlayer Sprite;
        public ActionState CurrentActionState = ActionState.Idle;
        public int Health { get; set; }
        public int Mana { get; set; }
        public int MotionSpeed { get; set; }
        public SpriteEffects CharSpriteInvertState = SpriteEffects.None; // CharSprite == Left || CharSprite == Right
        public Vector2 WorldPosition;
        public int BoundingRadius { get; set; }
        public Rectangle BoundingBox =>  new Rectangle((int)(WorldPosition.X - (BoundingRadius / 2)), (int) (WorldPosition.Y - BoundingRadius), BoundingRadius, BoundingRadius);
       
        public void DrawBoundingBox() => RenderFactory.DrawBox(BoundingBox, Color.LightGreen);

        public virtual void Draw(GameTime gtime, SpriteBatch sBatch)
        {
            if (Engine.Settings.Rendering.IsBoundingBoxRendered)
            {
                if (this.ObjectType == ObjectClientType.BaseObject && IsAlive)
                    DrawBoundingBox();
                else if (this.ObjectType == ObjectClientType.Spell)
                    DrawBoundingBox();
            }
        }

        public virtual void Update()
        {
            CheckEventStates();
        }

        public virtual void LoadContent() { }

        #region ClientEvents
        public delegate void ClientEventTemplate();
        public event ClientEventTemplate OnSpawn;
        public event ClientEventTemplate OnDeath;

        private void CheckEventStates()
        {
            if (OnSpawn != null && IsSpawned)
                OnSpawn.Invoke();

            if (OnDeath != null && !IsAlive)
                OnDeath.Invoke();
        }
        #endregion

        public enum ActionState
        {
            Idle,
            Walk,
            Jump,
            BasicAttack
        }

        public enum ObjectClientType
        {
            BaseObject,
            LocalPlayer,
            Spell
        }
    }
}
