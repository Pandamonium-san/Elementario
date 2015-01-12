using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elementario
{
    public static class SpriteRegions
    {

        public static Rectangle WallTower = new Rectangle(0, 0, 48, 48);
        public static Rectangle FireTower = new Rectangle(48, 0, 48, 48);
        public static Rectangle WaterTower = new Rectangle(96, 0, 48, 48);
        public static Rectangle EarthTower = new Rectangle(144, 0, 48, 48);
        public static Rectangle WindTower = new Rectangle(192, 0, 48, 48);
        public static Rectangle BladeTower = new Rectangle(240, 0, 48, 48);

        public static Rectangle RedSquare = new Rectangle(0, 49, 24, 24);
        public static Rectangle GreenCrystal = new Rectangle(24, 49, 24, 24);
        public static Rectangle BlueRing = new Rectangle(48, 49, 24, 24);
        public static Rectangle TreeDoor = new Rectangle(72, 49, 24, 24);
        public static Rectangle Mecha = new Rectangle(96, 49, 24, 24);
        public static Rectangle Wizard = new Rectangle(0, 73, 24, 24);
        public static Rectangle Ninja = new Rectangle(24, 73, 24, 24);
        public static Rectangle Thingy = new Rectangle(48, 73, 24, 24);
        public static Rectangle Thanatos = new Rectangle(72, 73, 24, 24);

        public static Rectangle Bullet = new Rectangle(0, 97, 12, 12);
        public static Rectangle WindBullet = new Rectangle(12, 97, 12, 12);
        public static Rectangle FireBullet = new Rectangle(0, 110, 12, 12);
        public static Rectangle Boulder = new Rectangle(24, 96, 19, 19);
        public static Rectangle Shuriken = new Rectangle(12, 110, 12, 12);
    }
}
