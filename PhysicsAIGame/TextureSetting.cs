using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace General
{
    // For the creation of textures that are mainly just solid blocks of colour
    static class TextureSetting
    {
        // Sets up a texture
        public static Texture2D SetTexture(GraphicsDevice gDevice, float width, float height, Color mainColour)
        {
            Texture2D texture = new Texture2D(gDevice, (int)width, (int)height);

            Color[] data = new Color[(int)width * (int)height];

            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = mainColour;
            }
            texture.SetData(data);

            return texture;
        }

        // Sets up a texture with a border - Currently unused
        public static Texture2D SetTextureWithBorder(GraphicsDevice gDevice, float width, float height, Color mainColour, float borderWidth, Color borderColour)
        {
            Texture2D texture = new Texture2D(gDevice, (int)width, (int)height);
            Color[] data = new Color[(int)width * (int)height];

            // ALL OF THE LOOPS
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    bool pixelIsColoured = false;

                    // Set the border pixels colour
                    for (int k = 0; k < borderWidth; k++)
                    {
                        if (i == k ||
                            j == k ||
                            i == width - 1 - k ||
                            j == height - 1 - k)
                        {
                            data[i + j * (int)width] = borderColour;
                            pixelIsColoured = true;
                            break;
                        }
                    }

                    // Set the rest of the pixels to the main colour
                    if (!pixelIsColoured)
                    {
                        data[i + j * (int)width] = mainColour;
                    }
                }
            }
            texture.SetData(data);

            return texture;
        }
    }
}
