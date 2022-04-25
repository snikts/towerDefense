using System;
using System.Collections.Generic;
using System.Text;

namespace CS5410.Objects
{
    public class GameCell
    {
        public int m_originx;
        public int m_originy;
        public int m_centerx;
        public int m_centery;
        private AnimatedSprite m_gameObject;
        public int m_distance;
        public bool visited;

        public GameCell(int x, int y, int centerX, int centerY, int distance)
        {
            m_originx = x;
            m_originy = y;
            m_centerx = centerX;
            m_centery = centerY;
            m_distance = distance;
            visited = false;
        }

        public bool occupied()
        {
            if(m_gameObject == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void setObject(AnimatedSprite gameObject)
        {
            m_gameObject = gameObject;
        }

        public AnimatedSprite getObject()
        {
            return m_gameObject;
        }

    }
}
