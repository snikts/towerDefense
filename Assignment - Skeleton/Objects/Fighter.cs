using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CS5410.Objects
{
    class Fighter : AnimatedSprite
    {

        public double m_swipeRate;
        private readonly double m_rotateRate;
        //public double m_radius;
        public int damage;
        private double lastSwipe = -1;
        private DateTime lastSwipeTime;
        //public int level;

        public Fighter(Vector2 size, Vector2 center, double swipeRate, double rotateRate, int radius) : base(size, center, radius, 0)
        {
            m_swipeRate = swipeRate;
            m_rotateRate = rotateRate;
            //m_radius = radius;
            damage = 10;
            //level = 1;
        }

        public void rotateLeft(GameTime gameTime)
        {
            m_rotation -= (float)(m_rotateRate * gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public void rotateRight(GameTime gameTime)
        {
            m_rotation += (float)(m_rotateRate * gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public bool canSwipe()
        {
            if(lastSwipe == -1)
            {
                lastSwipe = 0;
                lastSwipeTime = DateTime.Now;
                return true;
            }
            else
            {
                DateTime currTime = DateTime.Now;
                lastSwipe = (currTime - lastSwipeTime).TotalMilliseconds;
                if(lastSwipe >= (1000 / m_swipeRate))
                {
                    lastSwipeTime = DateTime.Now;
                    lastSwipe = 0;
                    return true;
                }
                return false;
            }
        }

        // Circle and Rectangle collision code modified from: http://www.jeffreythompson.org/collision-detection/circle-rect.php
        public bool doIntersect(Vector2 origin, Vector2 size)
        {
            // temporary variables to set edges for testing
            float testX = m_center.X;
            float testY = m_center.Y;

            // which edge is closest?
            if (m_center.X < origin.X)
            {
                testX = origin.X;      // test left edge
            }
            else if (m_center.X > origin.X + size.X)
            {
                testX = origin.X + size.X;   // right edge
            }
            if (m_center.Y < origin.Y)
            {
                testY = origin.Y;      // top edge
            }
            else if (m_center.Y > origin.Y + size.Y)
            {
                testY = origin.Y + size.Y;   // bottom edge
            }

            // get distance from closest edges
            float distX = m_center.X - testX;
            float distY = m_center.Y - testY;
            double distance = Math.Sqrt((distX * distX) + (distY * distY));

            // if the distance is less than the radius, collision!
            if (distance <= m_radius)
            {
                return true;
            }
            return false;
        }


    }
}
