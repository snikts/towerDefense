using System;
using Microsoft.Xna.Framework;
namespace CS5410.Objects
{
    public class Player
    {
        private double m_score;
        private double m_money;
        private Vector2 m_center;
        private Vector2 m_size;
        public Player(double score, double money, Vector2 center, Vector2 size)
        {
            m_score = score;
            m_money = money;
            m_center = center;
            m_size = size;
        }

        public Vector2 getPos()
        {
            return m_center;
        }
        
        public void setPos(Vector2 newPos)
        {
            m_center = newPos;
        }

        public void addMoney(double money)
        {
            m_money = m_money + money;
        }

        public double getMoney()
        {
            return m_money;
        }

        public void addToScore(double score)
        {
            m_score = m_score + score;
        }

        public double getScore()
        {
            return m_score;
        }
    }
}
