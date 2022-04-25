using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CS5410.Objects
{
    public class AnimatedSprite
    {
        private readonly Vector2 m_size;
        protected Vector2 m_center;
        protected float m_rotation = 0;

        public int location;
        public List<GameCell> shortestPath;

        public int level;
        public AnimatedSprite target;
        public int m_radius;
        public int m_health;

        public AnimatedSprite(Vector2 size, Vector2 center, int radius, int health)
        {
            m_size = size;
            m_center = center;
            location = 0;
            shortestPath = new List<GameCell>();
            level = 1;
            target = null;
            m_radius = radius;
            m_health = health;
        }

        public Vector2 Size
        {
            get { return m_size; }
        }

        public Vector2 Center
        {
            get { return m_center; }
        }

        public float Rotation
        {
            get { return m_rotation; }
            set { m_rotation = value; }
        }

        public void setX(int x)
        {
            m_center.X = x;
        }

        public void setY(int y)
        {
            m_center.Y = y;
        }

        public void increaseX(int x)
        {
            m_center.X = m_center.X + x;
        }

        public void increaseY(int y)
        {
            m_center.Y = m_center.Y + y;
        }

        public List<GameCell> getPath()
        {
            return shortestPath;
        }

        public void rotate(float rotation)
        {
            if (m_rotation < (rotation - .5))
            {
                m_rotation = m_rotation + .1f;
            }
            else if(m_rotation > (rotation + .5)) 
            {
                m_rotation = m_rotation - .1f;
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
            if (distance <= m_radius*64)
            {
                return true;
            }
            return false;
        }



    }
}
