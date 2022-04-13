using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CS5410.Objects
{
    class Dragon : AnimatedSprite
    {
        private readonly int m_moveRate;
        private readonly int m_health;

        public Dragon(Vector2 size, Vector2 center, int moveRate, int health) : base(size, center)
        {
            m_moveRate = moveRate;
            m_health = health;
        }
    }
}