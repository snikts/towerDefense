using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CS5410.Objects
{
    class Score : AnimatedSprite
    {
        public DateTime spawned;
        public int m_value;
        public Score(Vector2 size, Vector2 center, int value) : base(size, center, 0, 0)
        {
            spawned = DateTime.Now;
            m_value = value;
        }

        public bool move()
        {
            if((DateTime.Now-spawned).TotalSeconds < 1)
            {
                m_center.Y-=1f;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
