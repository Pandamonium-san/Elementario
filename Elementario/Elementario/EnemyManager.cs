using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Elementario
{
    public class EnemyManager
    {
        public List<Enemy> enemies, enemyQueue;
        public List<Node> keyNodes;
        public Node spawnNode1, endNode1, spawnNode2, endNode2;
        public bool started, victory;
        public int wave, playerLives, enemiesKilled, totalWavesToSpawn, enemyLevel;
        public float secsToNextLevel, secsToNextEnemySpawn, secsBetweenLevels, secsBetweenEnemySpawns;


        public EnemyManager()
        {
            started = false;
            enemies = new List<Enemy>();
            enemyQueue = new List<Enemy>();
            keyNodes = new List<Node>();
            totalWavesToSpawn = 10;
            secsBetweenEnemySpawns = 0.4f;
            secsBetweenLevels = 30;
            enemiesKilled = 0;

            playerLives = 25;

            keyNodes.Add(spawnNode1 = Game1.grid.nodes[0, 12]);
            keyNodes.Add(spawnNode2 = Game1.grid.nodes[17, 0]);
            keyNodes.Add(endNode1 = Game1.grid.nodes[34, 12]);
            keyNodes.Add(endNode2 = Game1.grid.nodes[17, 24]);
        }

        public void Update(GameTime gameTime)
        {
            if(started && (wave <= totalWavesToSpawn || victory))
            {
                if (wave < totalWavesToSpawn)
                    secsToNextLevel -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                secsToNextEnemySpawn -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (secsToNextLevel <= 0)
                    SendNextWave();
                if (secsToNextEnemySpawn <= 0)
                    SpawnNextEnemy();
            }
            if (enemyQueue.Count() == 0 && enemies.Count() == 0 && wave == totalWavesToSpawn)
            {
                totalWavesToSpawn = 999999;
                victory = true;
            }

            for (int i = 0; i < enemies.Count(); i++)
			{
                enemies[i].Update(gameTime);

                if (enemies[i].DestinationReached())
                {
                    --playerLives;
                    enemies.Remove(enemies[i]);
                    //--i;
                    continue;
                }
                else if (enemies[i].dead)
                {
                    ++enemiesKilled;
                    Game1.towerManager.resource += enemies[i].bounty;
                    enemies.Remove(enemies[i]);
                    --i;
                }
            }

        }

        public void SendNextWave()
        {
            if (wave >= totalWavesToSpawn)
                return;
            secsToNextLevel = secsBetweenLevels;
            ++wave;
            ++enemyLevel;
            AddEnemyWaveToQueue();
        }

        public void AddEnemyWaveToQueue()
        {
            int type = 0;
            if (wave % 10 == 0)
                type = 4;
            else if (wave % 7 == 0)
                type = 3;
            else if (wave % 6 == 0)
                type = 1;
            else if (wave % 4 == 0)
                type = 2;
            switch (type)
            {
                case 1:
                    for (int i = 0; i < 10; i++)
                    {
                    enemyQueue.Add(new FastEnemy(Game1.spriteSheet, new Rectangle(24, 49, 24, 24), spawnNode1, endNode1, enemyLevel));
                    enemyQueue.Add(new FastEnemy(Game1.spriteSheet, new Rectangle(24, 49, 24, 24), spawnNode2, endNode2, enemyLevel));
                    }
                    break;
                case 2:
                    for (int i = 0; i < 30; i++)
                    {
                    enemyQueue.Add(new SmallEnemy(Game1.spriteSheet, new Rectangle(48, 49, 24, 24), spawnNode1, endNode1, enemyLevel));
                    enemyQueue.Add(new SmallEnemy(Game1.spriteSheet, new Rectangle(48, 49, 24, 24), spawnNode2, endNode2, enemyLevel));
                    }
                    break;
                case 3:
                    for (int i = 0; i < 5; i++)
                    {
                        enemyQueue.Add(new BigEnemy(Game1.spriteSheet, new Rectangle(72, 49, 24, 24), spawnNode1, endNode1, enemyLevel));
                        enemyQueue.Add(new BigEnemy(Game1.spriteSheet, new Rectangle(72, 49, 24, 24), spawnNode2, endNode2, enemyLevel));
                    }
                    break;
                case 4:
                    for (int i = 0; i < 1; i++)
                    {
                        enemyQueue.Add(new BossEnemy(Game1.spriteSheet, new Rectangle(96, 49, 24, 24), spawnNode1, endNode1, enemyLevel));
                        enemyQueue.Add(new BossEnemy(Game1.spriteSheet, new Rectangle(96, 49, 24, 24), spawnNode2, endNode2, enemyLevel));
                    }
                    enemyLevel += 5;
                    break;
                default:
                    for (int i = 0; i < 10; i++)
                    {
                        enemyQueue.Add(new Enemy(Game1.spriteSheet, new Rectangle(0, 49, 24, 24), spawnNode1, endNode1, enemyLevel));
                        enemyQueue.Add(new Enemy(Game1.spriteSheet, new Rectangle(0, 49, 24, 24), spawnNode2, endNode2, enemyLevel));
                    }
                    break;
            }
        }

        public void SpawnNextEnemy()
        {
            if (enemyQueue.Count() == 0)
                return;
            if (enemyQueue[0] is SmallEnemy)
                secsBetweenEnemySpawns = 0.1f;
            else if (enemyQueue.Count >= 20)
                secsBetweenEnemySpawns = 0.2f;
            else
                secsBetweenEnemySpawns = 0.3f;
            enemies.Add(enemyQueue[0]);
            enemies.Add(enemyQueue[1]);
            enemyQueue[0].UpdatePath();
            enemyQueue[1].UpdatePath();
            enemyQueue.RemoveAt(1);
            enemyQueue.RemoveAt(0);

            secsToNextEnemySpawn = secsBetweenEnemySpawns;
        }

        public void UpdatePaths()
        {
            Game1.grid.UpdateNodes();
            foreach (Enemy e in enemies)
                e.UpdatePath();
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (Node n in keyNodes)
                spriteBatch.Draw(Game1.colorTexture, n.hitbox, Color.Blue);
            foreach(Enemy e in enemies)
            {
                e.Draw(spriteBatch);
                //if (e.path != null)   //Draw enemy path
                //    foreach (Node n in e.path)
                //        spriteBatch.Draw(Game1.colorTexture, n.hitbox, Color.Blue);
            }
        }

    }
}
