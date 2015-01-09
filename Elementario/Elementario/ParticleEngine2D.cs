using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elementario
{
    public class ParticleEngine2D
    {
        private Random rnd;
        GraphicsDevice graphicsDevice;
        List<Particle> particles;

        public ParticleEngine2D(GraphicsDevice graphicsDevice)
        {
            rnd = new Random();
            particles = new List<Particle>();
            this.graphicsDevice = graphicsDevice;
        }

        public void CreateParticle(Texture2D tex, Vector2 pos, Rectangle spriteRec, Vector2 velocity, Color color, float size, float lifeTime)
        {
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(rnd.NextDouble()*2-1);

            particles.Add(new Particle(tex, pos, spriteRec, velocity, angle, angularVelocity, size, color, lifeTime));
        }

        public void CreateExplosion(Vector2 pos, float radius, float lifeTime, Color color)
        {
            Texture2D tex = Game1.CreateCircleTex((int)(radius * 2), graphicsDevice);
            particles.Add(new Particle(tex, pos, new Rectangle(0, 0, tex.Width, tex.Height), Vector2.Zero, 0f, 0f, 1f, color, lifeTime));
        }

        public void Update(GameTime gameTime)
        {
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update(gameTime);
                if (particles[particle].lifeTime <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle p in particles)
                p.Draw(spriteBatch);
        }
    }
}
