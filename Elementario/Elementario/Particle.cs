using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elementario
{
    class Particle:Sprite
    {
        public Vector2 velocity;
        public float angle, angularVelocity;
        public float size;
        public float lifeTime, maxLifeTime;

        public Particle(Texture2D tex, Vector2 pos, Rectangle spriteRec, Vector2 velocity,
            float angle, float angularVelocity, float size, Color color, float lifeTime)
            : base(tex, pos, spriteRec)
        {
            this.velocity = velocity;
            this.angle = angle;
            this.angularVelocity = angularVelocity;
            this.size = size;
            this.color = color;
            this.lifeTime = lifeTime;
            maxLifeTime = lifeTime;
        }

        public void Update(GameTime gameTime)
        {
            lifeTime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            pos += velocity;
            angle += angularVelocity;
            alpha = lifeTime / maxLifeTime;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos, spriteRec, color*alpha, angle, origin, size, SpriteEffects.None, 0.7f);
        }

    }
}
