using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Aranslow.GameObjects
{
    /// <summary>
    /// Base class for all spell objects
    /// </summary>
    public class ASSpellClient : ASBaseClient
    {
        public ASSpellClient(Vector2 wPosition) : base(wPosition)
        {
            StartTime = Engine.SecondaryGameTimeHandle.TotalGameTime.TotalMilliseconds;
            StartPosition = wPosition;

            this.ObjectType = ObjectClientType.Spell;
        }

        public SpellSlot? Slot;
        public int MagicDamage;
        public int MissileSpeed
        {
            get => MotionSpeed;
            set => MotionSpeed = value;
        }
        public double StartTime { get; private set; }
        public double EndTime;
        public Vector2 StartPosition;
        public Vector2 EndPosition;
        public Vector2 DirectionHeadingTo;
        public Vector2 Velocity;

        public enum SpellSlot
        {
            BasicAttack
        }
    }
}
