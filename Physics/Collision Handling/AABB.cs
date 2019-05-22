using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Physics
{
    class AABB
    {
        public Vector2 Min { get; set; } // Top left corner
        public Vector2 Max { get; set; } // Bottom right corner
    }
}
