using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Elementario
{
    public class Game1 : Microsoft.Xna.Framework.Game       //TODO: Add sound
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Texture2D colorTexture, spriteSheet;
        public static SpriteFont titleFont, font,font2, font3;
        public static Random rnd = new Random();
        public enum GameState { Title, Playing, Paused }
        GameState gameState = GameState.Title;

        public static Grid grid;
        public static TowerManager towerManager;
        public static EnemyManager enemyManager;
        public static HUD hud;
        public static ParticleEngine2D particleEngine;
        Menu menu;

        public static bool showNodes;
        bool alreadyWon, debugMode;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1080;
            graphics.PreferredBackBufferHeight = 720;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            colorTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            colorTexture.SetData<Color>(new Color[] { Color.White });

            font = Content.Load<SpriteFont>("font");
            font2 = Content.Load<SpriteFont>("font2");
            font3 = Content.Load<SpriteFont>("font3");
            titleFont = Content.Load<SpriteFont>("titleFont");

            spriteSheet = Content.Load<Texture2D>("td_spriteSheet");

            grid = new Grid();
            towerManager = new TowerManager(GraphicsDevice);
            hud = new HUD(1080, 720);
            enemyManager = new EnemyManager();
            particleEngine = new ParticleEngine2D(GraphicsDevice);
            menu = new Menu(Window);
        }

        private void Play()
        {
            grid = new Grid();
            towerManager = new TowerManager(GraphicsDevice);
            hud = new HUD(1080, 720);
            enemyManager = new EnemyManager();
            gameState = GameState.Playing;
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            KeyMouseReader.Update();
            switch (gameState)
            {
                case GameState.Title:
                    menu.Update();
                    if (menu.playButton.ButtonClicked())
                        Play();
                    if (menu.exitButton.ButtonClicked() && menu.screen == Menu.Screen.gameOver)
                        menu.LoadTitleScreen();
                    else if (menu.exitButton.ButtonClicked())
                        this.Exit();
                    break;
                case GameState.Playing:
                    hud.Update();
                    towerManager.Update(gameTime, Window, grid);
                    enemyManager.Update(gameTime);
                    particleEngine.Update(gameTime);
                    if (KeyMouseReader.KeyPressed(Keys.P))
                    {
                        menu.LoadPauseScreen();
                        gameState = GameState.Paused;
                    }
                    if (enemyManager.playerLives <= 0)
                    {
                        gameState = GameState.Title;
                        menu.LoadGameOverScreen(false);
                    }
                    if (enemyManager.victory && !alreadyWon)
                    {
                        gameState = GameState.Paused;
                        menu.LoadGameOverScreen(true);
                        alreadyWon = true;
                    }
                    break;
                case GameState.Paused:
                    menu.Update();
                    if (KeyMouseReader.KeyPressed(Keys.P) || menu.playButton.ButtonClicked())
                    {
                        gameState = GameState.Playing;
                    }
                    if (menu.exitButton.ButtonClicked())
                    {
                        gameState = GameState.Title;
                        menu.LoadTitleScreen();
                    }
                    break;
                default:
                    break;
            }
            if (KeyMouseReader.KeyPressed(Keys.F12))
                debugMode = !debugMode;
            if (debugMode)
                Debug();

            base.Update(gameTime);
        }

        private void Debug()
        {
            if (KeyMouseReader.KeyPressed(Keys.E))
                enemyManager.enemies.Add(new Enemy(spriteSheet, new Rectangle(0, 49, 24, 24), enemyManager.spawnNode1, enemyManager.endNode1, 1));
            if (KeyMouseReader.KeyPressed(Keys.F1))
                towerManager.resource += 100000;

            if (KeyMouseReader.KeyPressed(Keys.F11))
                showNodes = !showNodes;
            if (Keyboard.GetState().IsKeyDown(Keys.F2))
                PathFinder.SearchStep();
            if (KeyMouseReader.KeyPressed(Keys.F3))
                PathFinder.SearchStep();
            if (KeyMouseReader.KeyPressed(Keys.Add))
                PathFinder.I += 1000;
            if (KeyMouseReader.KeyPressed(Keys.Subtract))
                PathFinder.I -= 1000;
        }

        private void DrawDebug(SpriteBatch spriteBatch)
        {
            Node hoverNode = null;
            foreach (Node n in grid.nodes)
            {
                if (n.hitbox.Contains(KeyMouseReader.mousePos))
                    hoverNode = n;
            }
            spriteBatch.DrawString(Game1.font3, "DebugModeActive", new Vector2(50, 630), Color.White);
            spriteBatch.DrawString(Game1.font3, "EnemyQueueCount " + enemyManager.enemyQueue.Count().ToString(), new Vector2(50, 615), Color.White);
            spriteBatch.DrawString(Game1.font3, "PathFinderIterations " + PathFinder.I.ToString(), new Vector2(50, 600), Color.White);
            if (hoverNode != null)
            {
                spriteBatch.DrawString(Game1.font3, "Node F-value " + hoverNode.F.ToString(), new Vector2(50, 585), Color.White);
                spriteBatch.DrawString(Game1.font3, "Node XY (" + hoverNode.X.ToString() + ", " + hoverNode.Y.ToString() + ")", new Vector2(50, 570), Color.White);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            //towerManager.DrawTowersToRenderTarget(spriteBatch, GraphicsDevice); //Obsolete experimental code
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            grid.Draw(spriteBatch);
            towerManager.Draw(spriteBatch);
            enemyManager.Draw(spriteBatch);
            towerManager.DrawProjectiles(spriteBatch);
            particleEngine.Draw(spriteBatch);
            hud.Draw(spriteBatch);

            if (gameState == GameState.Title || gameState == GameState.Paused)
                menu.Draw(spriteBatch);

            if (debugMode)
                DrawDebug(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
        public static Texture2D CreateCircleTex(int diam, GraphicsDevice graphicsDevice)
        {
            if (diam > 1000)
                diam = 1000;
            Texture2D texture = new Texture2D(graphicsDevice, diam, diam);
            Color[] colorData = new Color[diam * diam];

            float radius = diam / 2f;

            for (int x = 0; x < diam; x++)
            {
                for (int y = 0; y < diam; y++)
                {
                    int index = x + y * diam;
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    if (pos.Length() <= radius)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}
