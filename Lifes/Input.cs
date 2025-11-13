using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable

namespace Lifes
{
    internal abstract class InputBase
    {
        protected Rectangle rectangle;
        protected string label;
        protected bool isActive;
        protected bool isHovered;

        public InputBase(Rectangle rectangle, string label)
        {
            this.rectangle = rectangle;
            this.label = label;
        }

        public virtual void Update(MouseState mouse, KeyboardState keyboard)
        {
            Point mousePos = new Point(mouse.X, mouse.Y);
            isHovered = rectangle.Contains(mousePos);
            if (isHovered && mouse.LeftButton == ButtonState.Pressed)
                isActive = true;
            else if (mouse.LeftButton == ButtonState.Pressed && !isHovered)
                isActive = false;
        }

        public abstract void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D texture);
    }

    internal class TextInput : InputBase
    {
        private string value = "";

        public TextInput(Rectangle rect, string label) : base(rect, label) { }

        public override void Update(MouseState mouse, KeyboardState keyboard)
        {
            base.Update(mouse, keyboard);

            if (isActive)
            {
                foreach (var key in keyboard.GetPressedKeys())
                {
                    if (key == Keys.Back && value.Length > 0)
                        value = value[..^1];
                    else if (key >= Keys.A && key <= Keys.Z)
                        value += key.ToString();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D texture)
        {
            var color = isActive ? Color.LightBlue : (isHovered ? Color.Gray : Color.White);
            spriteBatch.Draw(texture, rectangle, color);
            spriteBatch.DrawString(font, $"{label}: {value}", new Vector2(rectangle.X + 5, rectangle.Y + 5), Color.Black);
        }
    }

    internal class NumberInput : InputBase
    {
        private int value = 0;

        public NumberInput(Rectangle rect, string label) : base(rect, label) { }

        public override void Update(MouseState mouse, KeyboardState keyboard)
        {
            base.Update(mouse, keyboard);

            if (isActive)
            {
                if (keyboard.IsKeyDown(Keys.Up)) value++;
                if (keyboard.IsKeyDown(Keys.Down)) value--;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D texture)
        {
            var color = isActive ? Color.LightBlue : (isHovered ? Color.Gray : Color.White);
            spriteBatch.Draw(texture, rectangle, color);
            spriteBatch.DrawString(font, $"{label}: {value}", new Vector2(rectangle.X + 5, rectangle.Y + 5), Color.Black);
        }
    }

    internal class ButtonInput : InputBase
    {
        private Action? onClick;

        public ButtonInput(Rectangle rect, string label, Action? onClick = null) : base(rect, label)
        {
            this.onClick = onClick;
        }

        public override void Update(MouseState mouse, KeyboardState keyboard)
        {
            Point mousePos = new Point(mouse.X, mouse.Y);
            bool hovered = rectangle.Contains(mousePos);

            if (hovered && mouse.LeftButton == ButtonState.Pressed)
                onClick?.Invoke();
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D texture)
        {
            var color = isHovered ? Color.Gray : Color.White;
            spriteBatch.Draw(texture, rectangle, color);
            spriteBatch.DrawString(font, label, new Vector2(rectangle.X + 10, rectangle.Y + 10), Color.Black);
        }
    }


}
