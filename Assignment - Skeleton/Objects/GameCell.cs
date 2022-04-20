using System;
using System.Collections.Generic;
using System.Text;

namespace CS5410.Objects
{
    public class GameCell
    {
        public int m_x;
        public int m_y;
        private AnimatedSprite m_gameObject;
        public int m_distance;

        public GameCell(int x, int y, int distance)
        {
            m_x = x;
            m_y = y;
            m_distance = distance;
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
