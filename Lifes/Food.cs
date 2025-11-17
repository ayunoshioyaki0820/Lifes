using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lifes
{
    internal class Food
    {
        public string Name { get; set; }
        public int Calories { get; set; }
        public Texture2D texture { get; set; }
        public float expireTimer { get; set; } // in seconds
        public Vector2 Position { get; set; }
        public bool isCanEat = true;
        public Color BaseColor, SecondColor;
        public Food(string name, int calories, float expiration, byte[,] looks, Vector2 position, GraphicsDevice device, Color BaseColor = default, Color SecondColor = default)
        {
            Name = name;
            Calories = calories;
            expireTimer = expiration;
            Position = position;
            
            if(BaseColor == default)
                this.BaseColor = Color.Brown;
            else
                this.BaseColor = BaseColor;
            if (SecondColor == default)
                this.SecondColor = Color.Green;
            else
                this.SecondColor = SecondColor;
            texture = SetColorData(looks, device);
        }
        private Texture2D SetColorData(byte[,] textures, GraphicsDevice device)
        {
            int width = textures.GetLength(0);
            int height = textures.GetLength(1);
            Color[] data = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;

                    // (★修正) looks の値 (0～3) を取得
                    byte lookValue = textures[x, y];

                    // (★修正) 0.5 のランダムではなく、lookValue に応じて色を決定
                    switch (lookValue)
                    {
                        case 0:
                            data[index] = Color.Transparent; // 0 = 透明
                            break;
                        case 1:
                            data[index] = BaseColor; // 1 = 基本色
                            break;
                        case 2:
                            data[index] = SecondColor; // 2 = アクセント色
                            break;
                        case 3:
                            // 3 = 影色 (基本色を暗くする)
                            data[index] = new Color(BaseColor.R / 2, BaseColor.G / 2, BaseColor.B / 2);
                            break;
                        default:
                            data[index] = Color.Transparent;
                            break;
                    }
                }
            }

            Texture2D tex = new Texture2D(device, width, height);
            tex.SetData(data);
            return tex;
        }

        public void Update(float deltaTime)
        {
            expireTimer -= deltaTime;
            if (expireTimer <= 0)
                isCanEat = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isCanEat)
            {
                spriteBatch.Draw(texture, Position, null, Color.Green, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            } else
            {
                spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            }
        }
    }
}
