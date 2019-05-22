using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace General
{
    // Taken and modified from the code provided on Course Resources to work with MonoGame vectors
    class Matrix2
    {
        public float[,] Mat = new float[2, 2] { { 1.0f, 0.0f }, { 0.0f, 1.0f } };

        public void SetRotation(float rad)
        {
            Mat[0, 0] = (float)Math.Cos(rad);
            Mat[0, 1] = -(float)Math.Sin(rad);

            Mat[1, 0] = (float)Math.Sin(rad);
            Mat[1, 1] = (float)Math.Cos(rad);
        }

        public static Vector2 operator *(Matrix2 mat, Vector2 vec)
        {
            Vector2 result = new Vector2();

            result.X = mat.Mat[0, 0] * vec.X + mat.Mat[0, 1] * vec.Y;
            result.Y = mat.Mat[1, 0] * vec.X + mat.Mat[1, 1] * vec.Y;

            return result;
        }
    }
}
