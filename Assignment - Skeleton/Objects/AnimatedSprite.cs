using Microsoft.Xna.Framework;
using System.Collections.Generic;

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

        public AnimatedSprite(Vector2 size, Vector2 center)
        {
            m_size = size;
            m_center = center;
            location = 0;
            shortestPath = new List<GameCell>();
            level = 1;
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



    }
}
