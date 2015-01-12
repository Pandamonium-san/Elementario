using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Elementario
{
    public class Sprite
    {
        public Texture2D tex;
        public Vector2 pos, origin;
        public Rectangle spriteRec;
        public Color color = Color.White;
        public float alpha = 1f;
        public float scale = 1f;
        public float rotation = 0f;

        public Sprite(Texture2D tex, Vector2 pos, Rectangle spriteRec)
        {
            this.tex = tex;
            this.pos = pos;
            this.spriteRec = spriteRec;
            origin = new Vector2(spriteRec.Width / 2, spriteRec.Height / 2);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos, spriteRec, color * alpha, rotation, origin, scale, SpriteEffects.None, 0f);
        }

    }
}
