using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elementario
{
    class Menu
    {
        float centerX, centerY;

        public TextButton playButton, exitButton;

        public enum Screen { title, gameOver, paused }
        public Screen screen;

        string titleText = "ELEMENTARIO";
        string gameOverText;

        public Menu(GameWindow window)
        {
            centerX = window.ClientBounds.Width / 2;
            centerY = window.ClientBounds.Height / 2;
            LoadTitleScreen();
        }

        public void Update()
        {
            playButton.Update();
            exitButton.Update();
        }

        public void LoadTitleScreen()
        {
            playButton = new TextButton(Game1.font, new Vector2(centerX, centerY - 50), "Start", 1f);
            exitButton = new TextButton(Game1.font, new Vector2(centerX, centerY), "Exit", 1f);
            screen = Screen.title;
        }

        public void LoadPauseScreen()
        {
            playButton = new TextButton(Game1.font, new Vector2(centerX, centerY - 50), "Resume", 1f);
            exitButton = new TextButton(Game1.font, new Vector2(centerX, centerY), "Back to main menu", 1f);
            screen = Screen.paused;
        }

        public void LoadGameOverScreen(bool win)
        {
            if (win)
            {
                playButton = new TextButton(Game1.font, new Vector2(centerX, centerY - 50), "Continue playing", 1f);
                gameOverText = "You win";
            }
            else
            {
                playButton = new TextButton(Game1.font, new Vector2(centerX, centerY - 50), "Try again", 1f);
                gameOverText = "You lose";
            }
            exitButton = new TextButton(Game1.font, new Vector2(centerX, centerY), "Back to main menu", 1f);
            screen = Screen.gameOver;
        }

        private void DrawStats(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.font, "Kills " + Game1.enemyManager.enemiesKilled.ToString(), new Vector2(centerX-400, centerY - 200), Color.White);
            spriteBatch.DrawString(Game1.font, "Towers " + Game1.towerManager.towers.Count().ToString(), new Vector2(centerX-400, centerY - 150), Color.White);

            int totalCost = 0;
            foreach (Tower t in Game1.towerManager.towers)
                totalCost += t.totalCost;
            spriteBatch.DrawString(Game1.font, "Resources spent " + totalCost.ToString(), new Vector2(centerX-400, centerY - 100), Color.White);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.colorTexture, new Rectangle(0, 0, (int)centerX * 2, (int)centerY * 2), Color.Black * 0.5f);

            switch (screen)
            {
                case Screen.title:
                    spriteBatch.DrawString(Game1.titleFont, titleText, new Vector2(centerX, centerY - 150) - Game1.titleFont.MeasureString(titleText) / 2, Color.White);
                    playButton.Draw(spriteBatch);
                    exitButton.Draw(spriteBatch);
                    break;
                case Screen.paused:
                    playButton.Draw(spriteBatch);
                    exitButton.Draw(spriteBatch);
                    break;
                case Screen.gameOver:
                    spriteBatch.DrawString(Game1.titleFont, gameOverText, new Vector2(centerX, centerY - 150) - Game1.titleFont.MeasureString(gameOverText) / 2, Color.White);
                    playButton.Draw(spriteBatch);
                    exitButton.Draw(spriteBatch);
                    DrawStats(spriteBatch);
                    break;
            }
        }
    }
}
