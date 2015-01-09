using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elementario
{
    public class TowerManager
    {
        public List<Tower> towers;
        GraphicsDevice graphicsDevice;
        RenderTarget2D towerRenderTarget;
        Texture2D rangeIndicator;
        public Tower ghostTower, activeTower;

        public int resource, interval;

        public enum Selection { None, Tower, Place }
        public Selection selection;

        public enum Place { Tower, FireTower, WaterTower, EarthTower, WindTower }
        public Place placeTool = Place.FireTower;

        public bool triedBlockingPath;

        public TowerManager(GraphicsDevice graphics)
        {
            towers = new List<Tower>();
            graphicsDevice = graphics;

            ghostTower = new Tower(Game1.spriteSheet, Vector2.Zero, new Rectangle(0,0,48,48));
            ghostTower.alpha = 0.5f;

            towerRenderTarget = new RenderTarget2D(graphics, graphics.PresentationParameters.BackBufferWidth, graphics.PresentationParameters.BackBufferHeight);
            resource = 100000;
        }

        public void Update(GameTime gameTime, GameWindow window, Grid grid)
        {
            TowerHotkeys();
            switch (selection)
            {
                case Selection.None:
                    foreach (Tower t in towers)
                        if (t.hitbox.Contains(KeyMouseReader.LeftClickPos))
                            SelectTower(t);
                    break;
                case Selection.Tower:
                    if (KeyMouseReader.LeftClick())
                    {
                        if (Game1.grid.gridRec.Contains(KeyMouseReader.LeftClickPos))
                            Deselect();
                        foreach (Tower t in towers)
                            if (t.hitbox.Contains(KeyMouseReader.LeftClickPos))
                                SelectTower(t);
                    }
                    break;
                case Selection.Place:
                    GhostTower(grid);
                    activeTower = ghostTower;
                    UpdateRangeIndicator();

                    if (KeyMouseReader.LeftClick())
                        PlaceTower(Game1.grid);
                    if (KeyMouseReader.RightClick())
                        selection = Selection.None;
                    break;
            }


            foreach (Tower t in towers)
                t.Update(gameTime);
        }

        private void TowerHotkeys()
        {
            if (KeyMouseReader.KeyPressed(Keys.D1))
            {
                Deselect();
                placeTool = (Place)0;
                selection = Selection.Place;
            }
            if (KeyMouseReader.KeyPressed(Keys.D2))
            {
                Deselect();
                placeTool = (Place)1;
                selection = Selection.Place;
            }
            if (KeyMouseReader.KeyPressed(Keys.D3))
            {
                Deselect();
                placeTool = (Place)2;
                selection = Selection.Place;
            }
            if (KeyMouseReader.KeyPressed(Keys.D4))
            {
                Deselect();
                placeTool = (Place)3;
                selection = Selection.Place;
            }
            if (KeyMouseReader.KeyPressed(Keys.D5))
            {
                Deselect();
                placeTool = (Place)4;
                selection = Selection.Place;
            }
        }

        private void SelectTower(Tower t)
        {
            activeTower = t;
            activeTower.color = new Color(155, 155, 100);
            UpdateRangeIndicator();
            selection = Selection.Tower;
        }

        public void Deselect()
        {
            if (activeTower != null)
                activeTower.color = Color.White;
            activeTower = new Tower(Game1.colorTexture, Vector2.Zero, Rectangle.Empty);
            activeTower.cost = 0;
            selection = Selection.None;
        }

        private void PlaceTower(Grid grid)
        {
            Tower t = GetTowerFromTool();

            if (resource < t.cost)
                return;

            SnapToGrid(t);
            towers.Add(t);
            Game1.enemyManager.UpdatePaths();

            if (!TowerHasValidPosition(t, grid))
            {
                towers.Remove(t);
                Game1.enemyManager.UpdatePaths();
                return;
            }

            t.totalCost += t.cost;
            resource -= t.cost;
        }

        public void SellTower(Tower t)
        {
            if (t == null)
                return;
            Deselect();
            resource += (int)(t.totalCost * 0.75f);
            towers.Remove(t);
            Game1.enemyManager.UpdatePaths();
        }

        public void UpgradeTower(Tower t)
        {
            if (resource < t.cost || !(t is Tower))
                return;
            resource -= t.cost;
            t.Upgrade();
            UpdateRangeIndicator();
        }

        public void SnapToGrid(Tower t)
        {
            t.pos.X = (int)((t.pos.X) / Grid.nodeSize) * Grid.nodeSize;
            t.pos.Y = (int)((t.pos.Y) / Grid.nodeSize) * Grid.nodeSize;
        }

        public void GhostTower(Grid grid)
        {
            ghostTower = GetTowerFromTool();
            ghostTower.pos = KeyMouseReader.mousePosV2;
            SnapToGrid(ghostTower);
            if (resource < ghostTower.cost || !TowerHasValidPosition(ghostTower, grid))
                ghostTower.color = new Color(255, 100, 100) * 0.5f;
            else
                ghostTower.color = Color.White * 0.5f;
        }

        private Tower GetTowerFromTool()
        {
            Tower t;
            switch (placeTool)
            {
                case Place.FireTower:
                    t = new FireTower(Game1.spriteSheet, ghostTower.pos, new Rectangle(48, 0, 48, 48));
                    break;
                case Place.WaterTower:
                    t = new WaterTower(Game1.spriteSheet, ghostTower.pos, new Rectangle(48 * 2, 0, 48, 48));
                    break;
                case Place.EarthTower:
                    t = new EarthTower(Game1.spriteSheet, ghostTower.pos, new Rectangle(48 * 3, 0, 48, 48));
                    break;
                case Place.WindTower:
                    t = new WindTower(Game1.spriteSheet, ghostTower.pos, new Rectangle(48 * 4, 0, 48, 48));
                    break;
                default:
                    t = new Tower(Game1.spriteSheet, ghostTower.pos, new Rectangle(0, 0, 48, 48));
                    break;
            }
            return t;
        }

        private bool TowerHasValidPosition(Tower t, Grid grid)
        {
            if(t != ghostTower)
            triedBlockingPath = false;

            if (!grid.gridRec.Contains(t.hitbox))   //Outside grid
                return false;

            foreach (Tower t2 in towers)
                if (t != t2 && t.hitbox.Intersects(t2.hitbox))      //Intersecting other tower
                    return false;

            foreach (Node n in Game1.enemyManager.keyNodes)         //Intersecting spawn node or end node
                if (t.hitbox.Intersects(n.hitbox))
                    return false;

            if (t != ghostTower &&
                (PathFinder.FindPath(Game1.enemyManager.spawnNode1, Game1.enemyManager.endNode1) == null ||
                PathFinder.FindPath(Game1.enemyManager.spawnNode2, Game1.enemyManager.endNode2) == null))   //Blocking path from spawn to finish
            {
                triedBlockingPath = true;
                return false;
            }

            foreach (Enemy e in Game1.enemyManager.enemies)     //Blocking path from enemy to finish
                if (e.path == null)
                {
                    triedBlockingPath = true;
                    return false;
                }

            //if (TowerPixelCollisionWithRenderTarget(t))   //Obsolete experimental code
            //    return false;
            return true;
        }

        private void UpdateRangeIndicator()
        {
            if (activeTower.range > 0)
                rangeIndicator = Game1.CreateCircleTex((int)activeTower.range * 2, graphicsDevice);
        }

        private bool TowerPixelCollisionWithRenderTarget(Tower t)   //Obsolete experimental code
        {
            Color[] dataA = new Color[towerRenderTarget.Width * towerRenderTarget.Height];
            towerRenderTarget.GetData(dataA);

            Color[] dataB = new Color[t.spriteRec.Width * t.spriteRec.Height];
            t.tex.GetData(0,
                t.spriteRec,
                dataB,
                0,
                t.spriteRec.Width * t.spriteRec.Height);

            for (int y = t.hitbox.Top; y < t.hitbox.Bottom; y++)
            {
                for (int x = t.hitbox.Left; x < t.hitbox.Right; x++)
                {
                    Color colorA = dataA[(x) + (y) * towerRenderTarget.Width];
                    Color colorB = dataB[(x - t.hitbox.Left) + (y - t.hitbox.Top) * t.hitbox.Width];

                    if (colorA.A != 0 && colorB.A != 0)
                        return true;
                }
            }
            return false;
        }

        public void DrawTowersToRenderTarget(SpriteBatch spriteBatch, GraphicsDevice graphics) //Obsolete experimental code
        {
            graphics.SetRenderTarget(towerRenderTarget);
            graphics.Clear(Color.Transparent);
            spriteBatch.Begin();
            foreach (Tower t in towers)
                t.Draw(spriteBatch);
            spriteBatch.End();
            graphics.SetRenderTarget(null);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(selection == Selection.Place)
            ghostTower.Draw(spriteBatch);
            foreach (Tower t in towers)
                t.Draw(spriteBatch);
            foreach (Tower t in towers)
                t.DrawProjectiles(spriteBatch);
            if (selection != Selection.None && activeTower != null && rangeIndicator != null && activeTower.range > 0)
                spriteBatch.Draw(rangeIndicator, activeTower.pos - new Vector2(activeTower.range,activeTower.range) + activeTower.origin, null, Color.Blue*0.2f, 0f, activeTower.origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
