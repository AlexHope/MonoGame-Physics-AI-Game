using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace General
{
    // Used when spawning enemy at the given position
    class SpawnPoint
    {
        public Vector2 Position { get; set; }
        public float TimeSinceLastSpawn { get; set; }

        private const int _respawnDelay = 10;

        public SpawnPoint(Vector2 position)
        {
            Position = position;
            TimeSinceLastSpawn = 7;
        }

        public int RespawnDelay
        {
            get
            {
                return _respawnDelay;
            }
        }
    }
}
