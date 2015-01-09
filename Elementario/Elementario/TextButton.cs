using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elementario
{
    public class TextButton:Button
    {
        public String name;
        SpriteFont font;
        float scale;

        public TextButton(SpriteFont font, Vector2 pos, String name, float scale):base(pos, 0, 0)
        {
            this.font = font;
            this.name = name;
            this.scale = scale;
            origin = font.MeasureString(name) / 2;

            this.rec = new Rectangle((int)(pos.X - origin.X*scale), (int)(pos.Y - origin.Y*scale), (int)(font.MeasureString(name).X*scale), (int)(font.MeasureString(name).Y*scale));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Game1.colorTexture, rec, Color.Red);
            spriteBatch.DrawString(font, name, pos, color, 0f, origin, scale, SpriteEffects.None, 0f);
        }
    }
}
