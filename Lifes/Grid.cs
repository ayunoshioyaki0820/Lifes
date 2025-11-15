using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Lifes
{    
    public enum GridOrientation
    {
        Horizontal,
        Vertical
    }
    internal class Grid<T> where T : InputBase
    {
        public SpriteFont font;
        List<T> inputs = [];
        GridOrientation position;
        int padding = 4;
        int width;
        public Color FontColor { get; set; } = Color.White;
        public Color BackGroundColor { get; set; } = Color.Black;
        internal int Top, Bottom, Left, Right;
        internal Grid(SpriteFont font, List<T> inputs, int width)
        {
            this.font = font;
            this.inputs.AddRange(inputs);
            this.width = width;
            this.position = GridOrientation.Vertical;
            this.Top = inputs[0].InputRect.Top;
            this.Bottom = inputs[0].InputRect.Bottom;
            this.Left = inputs[0].InputRect.Left;
            this.Right = inputs[inputs.Count - 1].InputRect.Right;
        }
        internal Grid(SpriteFont font, List<T> inputs, int width, GridOrientation position, int padding)
        {
            this.font = font;
            this.inputs.AddRange(inputs);
            this.width = width;
            this.position = position;
            this.Top = inputs[0].InputRect.Top;
            this.Bottom = inputs[0].InputRect.Bottom;
            this.Left = inputs[0].InputRect.Left;
            this.Right = inputs[inputs.Count - 1].InputRect.Right;
            this.padding = padding;
            if (position == GridOrientation.Horizontal)
            {
                LayoutHorizontal();
            }
            else
            {
                LayoutVertical();
            }
        }
        void LayoutHorizontal()
        {
            int count = inputs.Count;
            int totalPadding = padding * (count - 1);
            int eachWidth = (width - totalPadding) / count;

            int x = inputs[0].InputRect.X;
            int y = inputs[0].InputRect.Y;

            for (int i = 0; i < count; i++)
            {
                var input = inputs[i];
                input.InputRect = new Rectangle(
                    x + (eachWidth + padding) * i,
                    y,
                    eachWidth,
                    input.InputRect.Height
                );

                input.InitializeLayout(input.InputRect);
                input.LabelPoint = new Vector2(
                    input.InputRect.X,  y - input.fontHeight
                );
                Console.WriteLine(input.LabelPoint);
            }
        }

        void LayoutVertical()
        {
            int count = inputs.Count;
            for( int i = 0; i < count; i++)
            {
                var input = inputs[i];
                input.InputRect = new Rectangle(
                    inputs[0].InputRect.X,
                    inputs[0].InputRect.Y + (input.InputRect.Height + padding) * i,
                    width,
                    input.InputRect.Height
                );
                input.InitializeLayout(input.InputRect);
            }
        }
        public void RecalculateLayout()
        {
            if (position == GridOrientation.Horizontal)
                LayoutHorizontal();
            else
                LayoutVertical();
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
