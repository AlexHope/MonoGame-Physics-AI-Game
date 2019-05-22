using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace General
{
    class Camera
    {
        private Viewport _viewport;
        public Vector2 CameraPosition { get; set; }
        public Vector2 Origin { get; set; }

        public Camera(Viewport viewport)
        {
            _viewport = viewport;
            CameraPosition = new Vector2(0, 0);
            Origin = new Vector2(viewport.Width / 2, viewport.Height / 2);
        }

        public Matrix ViewMatrix()
        {
            return  Matrix.CreateTranslation(new Vector3(-CameraPosition, 0.0f)) *
                    Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                    Matrix.CreateRotationZ(0) *
                    Matrix.CreateScale(1.0f, 1.0f, 1.0f) *
                    Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }
    }
}
