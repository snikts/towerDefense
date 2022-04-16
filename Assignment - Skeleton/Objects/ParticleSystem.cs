using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CS5410.Objects
{
    public class ParticleSystem
    {
        private ContentManager m_content;
        private Texture2D[] m_textures;
        private List<ParticleEmitter> emitters;

        public ParticleSystem(ContentManager content, Texture2D[] particleTextures)
        {
            m_content = content;
            m_textures = particleTextures;
            emitters = new List<ParticleEmitter>();
        }

        public void creepDeath(TimeSpan rate, int sourceX, int sourceY, int size, int speed, TimeSpan lifetime)
        {
            emitters.Add(new ParticleEmitter(m_content, rate, sourceX, sourceY, size, speed, lifetime, m_textures[0]));
        }

        public void bombTrail(TimeSpan rate, int sourceX, int sourceY, int size, int speed, TimeSpan lifetime)
        {
            emitters.Add(new ParticleEmitter(m_content, rate, sourceX, sourceY, size, speed, lifetime, m_textures[1]));
        }

        public void bombHit(TimeSpan rate, int sourceX, int sourceY, int size, int speed, TimeSpan lifetime)
        {
            emitters.Add(new ParticleEmitter(m_content, rate, sourceX, sourceY, size, speed, lifetime, m_textures[2]));
        }

        public void missileTrail(TimeSpan rate, int sourceX, int sourceY, int size, int speed, TimeSpan lifetime)
        {
            emitters.Add(new ParticleEmitter(m_content, rate, sourceX, sourceY, size, speed, lifetime, m_textures[3]));
        }

        public void missileHit(TimeSpan rate, int sourceX, int sourceY, int size, int speed, TimeSpan lifetime)
        {
            emitters.Add(new ParticleEmitter(m_content, rate, sourceX, sourceY, size, speed, lifetime, m_textures[4]));
        }

        public void sellTower(TimeSpan rate, int sourceX, int sourceY, int size, int speed, TimeSpan lifetime)
        {
            emitters.Add(new ParticleEmitter(m_content, rate, sourceX, sourceY, size, speed, lifetime, m_textures[5]));
        }

        public void update(GameTime gameTime)
        {
            for (int i = 0; i < emitters.Count; i++)
            {
                emitters[i].update(gameTime);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < emitters.Count; i++)
            {
                emitters[i].draw(spriteBatch);
            }
        }
    }


}
