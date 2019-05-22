using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics;
using AI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace General
{
    class World
    {
        public Random RandomGen { get; set; }
        public PhysicsManager PhysicsManager { get; set; }
        public AIManager AIManager { get; set; }

        public float Score { get; set; }
        public float PortalHealth { get; set; }
        public bool GameOver { get; set; }

        public Character Player { get; set; }
        public PhysObject Target { get; set; }
        public List<Character> Enemies { get; set; }
        public List<SpawnPoint> SpawnPoints { get; set; }
        public List<SpawnPoint> ActiveSpawnPoints { get; set; }

        public int MaximumEnemies { get; set; }

        public bool AllowAIBehaviour { get; set; }
        public bool ProjectileCharacterCollision { get; set; }

        private const int _physicsSubstep = 10;

        public World(GraphicsDevice g)
        {
            RandomGen = new Random();
            PhysicsManager = new PhysicsManager(g, this);

            Enemies = new List<Character>();
            Player = PhysicsManager.Player;
            Target = PhysicsManager.Target;

            AIManager = new AIManager(this);

            MaximumEnemies = 5;
            Score = 0;
            PortalHealth = 100;
            GameOver = false;

            AllowAIBehaviour = true;
            ProjectileCharacterCollision = true;

            SpawnPoints = new List<SpawnPoint>();
            ActiveSpawnPoints = new List<SpawnPoint>();
            CreateSpawnPoints();
        }

        public void Update(GameTime gt)
        {
            float dt = (float)gt.ElapsedGameTime.TotalSeconds;

            // Update the physics multiple times per frame (in this case, 10 times per frame)
            for (int i = 0; i < _physicsSubstep; i++)
            {
                PhysicsManager.Update(dt / _physicsSubstep);
            }

            AIManager.Update(this, dt);

            // Check if there are less enemies than the maximum and add more as necessary
            if (Enemies.Count < MaximumEnemies)
            {
                SpawnEnemy();
            }

            // Update the spawn points
            foreach (SpawnPoint sp in SpawnPoints)
            {
                if (sp.TimeSinceLastSpawn > sp.RespawnDelay && !ActiveSpawnPoints.Contains(sp))
                {
                    ActiveSpawnPoints.Add(sp);
                }

                sp.TimeSinceLastSpawn += dt;
            }

            // Health shouldn't be below 0
            if (PortalHealth < 0)
            {
                PortalHealth = 0;
            }

            // If you haven't lost yet, keep updating the score
            if (!GameOver)
            {
                Score += dt;
            }
        }

        private void CreateSpawnPoints()
        {
            // Spawn Points at Tier 3
            SpawnPoint sp = new SpawnPoint(new Vector2(1024, 1936)); 
            SpawnPoints.Add(sp);

            // Spawn Points at Tier 4
            sp = new SpawnPoint(new Vector2(256, 1840));
            SpawnPoints.Add(sp);
            sp = new SpawnPoint(new Vector2(1792, 1840));
            SpawnPoints.Add(sp);

            // Spawn Points at Tier 8
            sp = new SpawnPoint(new Vector2(768, 1296));
            SpawnPoints.Add(sp);
            sp = new SpawnPoint(new Vector2(1280, 1296));
            SpawnPoints.Add(sp);

            // Spawn Points at Tier 9
            sp = new SpawnPoint(new Vector2(128, 1008));
            SpawnPoints.Add(sp);
            sp = new SpawnPoint(new Vector2(1920, 1008));
            SpawnPoints.Add(sp);

            // Spawn Points at Tier 10
            sp = new SpawnPoint(new Vector2(848, 752));
            SpawnPoints.Add(sp);
            sp = new SpawnPoint(new Vector2(1200, 752));
            SpawnPoints.Add(sp);
        }

        // Spawns an enemy
        public void SpawnEnemy()
        {
            if (Enemies.Count < MaximumEnemies && ActiveSpawnPoints.Count > 0)
            {
                SpawnPoint sp = ActiveSpawnPoints[RandomGen.Next(0, ActiveSpawnPoints.Count - 1)];

                Character enemy = new Character(false);
                enemy.Behaviour = new AIBehaviour(enemy, AIManager.NodeManager);
                enemy.Position = sp.Position;
                RigidBody enemyBody = enemy.RigidBody;
                enemyBody.Position = enemy.Position;
                enemyBody.Width = 32;
                enemyBody.Height = 32;
                enemyBody.CollidableObject = new BoxCollider();
                enemy.TexturePos = new Vector2(enemy.Position.X - enemyBody.Width / 2, enemy.Position.Y - enemyBody.Height / 2);
                PhysicsManager.DynamicObjects.Add(enemy);
                PhysicsManager.AllPhysObjects.Add(enemy);
                Enemies.Add(enemy);

                sp.TimeSinceLastSpawn = 0;
                ActiveSpawnPoints.Remove(sp);
            }
        }

        // DEBUG - Spawns an enemy at cursor
        public void DebugSpawnEnemy(Vector2 cursorPos)
        {
            if (Enemies.Count < MaximumEnemies)
            {
                Character enemy = new Character(false);
                enemy.Behaviour = new AIBehaviour(enemy, AIManager.NodeManager);
                enemy.Position = cursorPos;
                RigidBody enemyBody = enemy.RigidBody;
                enemyBody.Position = enemy.Position;
                enemyBody.Width = 32;
                enemyBody.Height = 32;
                enemyBody.CollidableObject = new BoxCollider();
                enemy.TexturePos = new Vector2(enemy.Position.X - enemyBody.Width / 2, enemy.Position.Y - enemyBody.Height / 2);
                PhysicsManager.DynamicObjects.Add(enemy);
                PhysicsManager.AllPhysObjects.Add(enemy);
                Enemies.Add(enemy);
            }
        }
    }
}
