using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elementario
{
    public class TextureButton:Button
    {
        public enum ButtonType { Tower, FireTower }
        public ButtonType type = ButtonType.Tower;
        Texture2D texture;
        Nullable<Rectangle> spriteRec = new Rectangle(0,0,32,32);

        public TextureButton(Texture2D texture, Nullable<Rectangle> spriteRec, Vector2 pos, int type):base(pos, 48, 48)
        {
            this.texture = texture;
            this.spriteRec = spriteRec;
            this.type = (ButtonType)type;
        }

        public override void Update()
        {
            if (rec.Contains(KeyMouseReader.mousePos))
                alpha = 1f;
            else
                alpha = 0.5F;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
            spritebatch.Draw(texture, rec, spriteRec, Color.White * alpha);
        }
    }
}
