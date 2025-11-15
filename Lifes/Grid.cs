using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifes
{
    internal class Grid<T> where T : InputBase
    {
        public SpriteFont font;
        List<T> inputs = new List<T>();
        int padding;
        public Color fontColor { get; set; } = Color.White;
        public Color backGroundColor { get; set; } = Color.Black;
        internal Grid(SpriteFont font, List<T> inputs, int padding = 4)
        {
            this.font = font;
            this.inputs.AddRange(inputs);
            this.padding = padding;
        }

        internal void Update(GameTime gameTime, KeyboardState key, MouseState mouse)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                inputs[i].Update(mouse, key, gameTime);
            }
        }

        internal void Draw(SpriteBatch _spriteBatch, Texture2D texture)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                inputs[i].Draw(_spriteBatch, texture);
            }
        }
    }
}
