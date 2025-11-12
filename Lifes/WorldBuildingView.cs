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
    internal class WorldBuildingView
    {
        private int focusIndex = 0;
        private string[] focusState = { "None", "Input" };
        private Texture2D whitePixel;
        Rectangle rectangle;
        InputForm inputBox;
        public WorldBuildingView(GraphicsDevice device, SpriteFont font)
        {
            whitePixel = new Texture2D(device, 1, 1);
            whitePixel.SetData(new[] { Color.White });
            var centerX = device.Viewport.Width / 2;
            rectangle = new Rectangle(centerX - 150, 50, 300, 40);
            inputBox = new InputForm(rectangle, font);
        }

        void Isfocus(MouseState mouse, Rectangle rect)
        {
            if(mouse.LeftButton == ButtonState.Released && GameManager.previousMouseState.LeftButton == ButtonState.Pressed)
            {
                if (rect.Contains(mouse.Position))
                {
                    focusIndex = 1; // Input
                }
                else
                {
                    focusIndex = 0; // None
                }
            }
        }


        public void Update(GameTime gameTime, KeyboardState key, MouseState mouse)
        {
            Isfocus(mouse, rectangle);
            inputBox.Update(key, mouse);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            var centerX = spriteBatch.GraphicsDevice.Viewport.Width / 2;
            var size = font.MeasureString("World Building View (est to back)");
            spriteBatch.DrawString(font, "World Building View (est to back)", new Vector2(centerX - size.X / 2, 20), Color.White);

            inputBox.Draw(spriteBatch, font, whitePixel, Color.White);
        }
    }
}
