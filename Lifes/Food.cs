using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
        public Food(string name, int calories, float expiration, byte[,] looks, GraphicsDevice device)
        {
            this.Name = name;
            this.Calories = calories;
            this.expireTimer = expiration;

            int width = looks.GetLength(0);
            int height = looks.GetLength(1);
            this.texture = new Texture2D(device, width, height); ;
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
