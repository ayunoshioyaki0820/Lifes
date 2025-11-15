using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Lifes
{
    internal class WorldBuildingView
    {
        private Texture2D whitePixel;
        Rectangle rectangle;
        TextInput inputBox;
        ButtonInput button;
        NumberInput numberInput;
        Grid<NumberInput> grid;

        GraphicsDevice device;
        SpriteFont font;
        int lastWidth;
        int lastHeight;
        public WorldBuildingView(GraphicsDevice device, SpriteFont font)
        {
            this.device = device;
            this.font = font;
            whitePixel = new Texture2D(device, 1, 1);
            whitePixel.SetData(new[] { Color.White });
            
            Layout();
        }

        private void Layout()
        {
            var centerX = device.Viewport.Width / 2;
            lastWidth = device.Viewport.Width;
            lastHeight = device.Viewport.Height;

            rectangle = new Rectangle(centerX / 2, 50, centerX, 40);

            inputBox = new TextInput(rectangle, "World Name", font);
            button = new ButtonInput(
                new Rectangle(centerX / 2, inputBox.Bottom, centerX, 40),
                "Create", font
            );
            numberInput = new NumberInput(
                new Rectangle(centerX / 2, button.Bottom, centerX, 40),
                "Size", font
            );

            var widthInput = new NumberInput(
                new Rectangle(centerX / 2, numberInput.Bottom, centerX, 40),
                "Width", font
            );
            var heightInput = new NumberInput(
                new Rectangle(centerX / 2, widthInput.Bottom, centerX, 40),
                "Height", font
            );

            grid = new Grid<NumberInput>(font, new List<NumberInput> { widthInput, heightInput }, 10);
        }


        public void Update(GameTime gameTime, KeyboardState key, MouseState mouse)
        {
            if (lastWidth != device.Viewport.Width || lastHeight != device.Viewport.Height)
            {
                Layout();
            }
            inputBox.Update(mouse, key, gameTime);
            button.Update(mouse, key, gameTime);
            numberInput.Update(mouse, key, gameTime);
            grid.Update(gameTime, key, mouse);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            var centerX = spriteBatch.GraphicsDevice.Viewport.Width / 2;
            var size = font.MeasureString("World Building View (est to back)");
            spriteBatch.DrawString(font, "World Building View (est to back)", new Vector2(centerX / 2, 20), Color.White);

            inputBox.Draw(spriteBatch, whitePixel);
            button.Draw(spriteBatch, whitePixel);
            numberInput.Draw(spriteBatch, whitePixel);
            grid.Draw(spriteBatch, whitePixel);
        }
    }
}
