using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elementario
{
    public class HUD
    {
        Rectangle rec;
        int hudWidth = 192;
        int windowX, windowY;

        public List<TextureButton> buttons;
        TextButton startButton, upgradeButton, sellButton;

        public bool pause;

        public HUD(int windowX, int windowY)
        {
            this.windowX = windowX;
            this.windowY = windowY;

            rec = new Rectangle(windowX - hudWidth, 0, hudWidth, windowY);

            buttons = new List<TextureButton>();

            int type = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (i + j == 3)
                        break;
                    buttons.Add(new TextureButton(Game1.spriteSheet, new Rectangle(0 + type * 48, 0, 48, 48), new Vector2(rec.X + 60 + j * 72, rec.Y + 260 + i*72), type));
                    ++type;
                }
            }

            upgradeButton = new TextButton(Game1.font, new Vector2(windowX - 110, windowY - 260), "Upgrade", 1f);
            sellButton = new TextButton(Game1.font, new Vector2(windowX - 127, windowY - 200), "Sell", 1f);
            startButton = new TextButton(Game1.font2, new Vector2(windowX - 90, windowY - 40), "START", 1f);
        }

        public void Update()
        {
            SellButton();
            StartButton();
            UpgradeButton();
            foreach (TextureButton b in buttons)
            {
                b.Update();
                if (b.ButtonClicked())
                {
                    Game1.towerManager.placeTool = (TowerManager.Place)b.type;
                    Game1.towerManager.Deselect();
                    Game1.towerManager.selection = TowerManager.Selection.Place;
                }
            }
        }

        private void SellButton()
        {
            sellButton.Update();
            if (sellButton.ButtonClicked() || KeyMouseReader.KeyPressed(Keys.S))
            {
                Game1.towerManager.SellTower(Game1.towerManager.activeTower);
            }
        }

        private void StartButton()
        {
            startButton.Update();
            if (startButton.ButtonClicked() || KeyMouseReader.KeyPressed(Keys.Space))
            {
                Game1.enemyManager.started = true;
                Game1.enemyManager.SendNextWave();
            }
        }

        private void UpgradeButton()
        {
            upgradeButton.Update();
            if(Game1.towerManager.selection == TowerManager.Selection.Tower &&
                (upgradeButton.ButtonClicked() || KeyMouseReader.KeyPressed(Keys.U)) &&
                Game1.towerManager.activeTower is Tower)
            Game1.towerManager.UpgradeTower((Tower)Game1.towerManager.activeTower);
        }

        private void DrawInfo(SpriteBatch spriteBatch)
        {
            string info = "";
            if (!Game1.enemyManager.started)
                info = "Enemies spawn on the top and left blue squares and move to the square opposite";
            else if (Game1.towerManager.triedBlockingPath)
                info = "Cannot place tower; blocking enemy path";
            else if ((Game1.enemyManager.wave + 1) % 10 == 0)
                info = "Prepare yourself for a tough one.";
            else if ((Game1.enemyManager.wave + 1) % 7 == 0)
                info = "Coming up... I'm not sure what that is.";
            else if ((Game1.enemyManager.wave + 1) % 6 == 0)
                info = "Next wave is speeeedy. Better get some Water.";
            else if ((Game1.enemyManager.wave + 1) % 4 == 0)
                info = "Bunch of little guys coming. Fire is effective.";

            spriteBatch.DrawString(Game1.font3, info, new Vector2(170, 15), Color.White);
        }

        private void DrawTowerInfo(SpriteBatch spriteBatch, Tower activeTower)
        {
            if (Game1.towerManager.activeTower != null)
            {
                upgradeButton.name = "Upgrade\n(" + Game1.towerManager.activeTower.upgradeCost.ToString() + ")";
                sellButton.name = "Sell\n(" + ((int)(Game1.towerManager.activeTower.totalCost * 0.75f)).ToString() + ")";
            }
            activeTower.DrawTowerInfo(spriteBatch, windowX, windowY);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawInfo(spriteBatch);
            foreach (TextureButton b in buttons)
                b.Draw(spriteBatch);

            startButton.Draw(spriteBatch);
            upgradeButton.Draw(spriteBatch);
            sellButton.Draw(spriteBatch);

            if(Game1.towerManager.activeTower != null && Game1.towerManager.selection != TowerManager.Selection.None)
            DrawTowerInfo(spriteBatch, Game1.towerManager.activeTower);
            spriteBatch.DrawString(Game1.font, "Lives " + ((int)Game1.enemyManager.playerLives).ToString(), new Vector2(20, 10), Color.White);
            spriteBatch.DrawString(Game1.font, "Current wave " + Game1.enemyManager.wave.ToString(), new Vector2(20, windowY - 50), Color.White);
            spriteBatch.DrawString(Game1.font, "Next wave spawns in " + ((int)Game1.enemyManager.secsToNextLevel).ToString(), new Vector2(500, windowY - 50), Color.White);
            spriteBatch.DrawString(Game1.font, "Resources " + "\n" + ((int)Game1.towerManager.resource).ToString(), new Vector2(windowX - 150, windowY - 150), Color.White);

        }
    }
}
