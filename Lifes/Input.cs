using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
#nullable enable

namespace Lifes
{
    internal abstract class InputBase
    {
        protected Rectangle InputRect;
        protected Vector2 LabelPoint;
        protected string label;
        protected bool isActive;
        protected bool isHovered;
        protected float time;
        protected SpriteFont font;
        protected int fontHeight;
        protected int Height;

        public InputBase(Rectangle InputRect, string label, SpriteFont font)
        {
            this.InputRect = InputRect;
            this.label = label;
            this.font = font;
            this.fontHeight = font.LineSpacing;
            this.Height = InputRect.Height;
            LabelPoint = new Vector2(InputRect.X, InputRect.Y);
            this.InputRect.Y -= fontHeight;
        }

        public virtual void Update(MouseState mouse, KeyboardState keyboard)
        {
            Point mousePos = new Point(mouse.X, mouse.Y);
            isHovered = InputRect.Contains(mousePos);
            if (isHovered && mouse.LeftButton == ButtonState.Pressed)
                isActive = true;
            else if (mouse.LeftButton == ButtonState.Pressed && !isHovered)
                isActive = false;
            time += 0.16f;
            if (time >= 1f)
                time = 0f;
        }

        public abstract void Draw(SpriteBatch spriteBatch, Texture2D texture);
    }

    internal class TextInput : InputBase
    {
        private string value = "";

        public TextInput(Rectangle rect, string label, SpriteFont font) : base(rect, label, font) {
            this.Height += fontHeight;
        }

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

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            var color = isActive ? Color.LightBlue : (isHovered ? Color.Gray : Color.White);
            spriteBatch.DrawString(font, label, LabelPoint, Color.Black);
            spriteBatch.Draw(texture, InputRect, color);
        }
    }

    internal class NumberInput : InputBase
    {
        private int value = 0;

        public NumberInput(Rectangle rect, string label, Vector2 point, SpriteFont font) : base(rect, label, font) { }

        public override void Update(MouseState mouse, KeyboardState keyboard)
        {
            base.Update(mouse, keyboard);

            if (isActive)
            {
                if (keyboard.IsKeyDown(Keys.Up)) value++;
                if (keyboard.IsKeyDown(Keys.Down)) value--;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            var color = isActive ? Color.LightBlue : (isHovered ? Color.Gray : Color.White);
            spriteBatch.Draw(texture, InputRect, color);
            spriteBatch.DrawString(font, $"{label}: {value}", new Vector2(InputRect.X + 5, InputRect.Y + 5), Color.Black);
        }
    }

    internal class ButtonInput : InputBase
    {
        private Action? onClick;

        public ButtonInput(Rectangle rect, string label, SpriteFont font, Action? onClick = null) : base(rect, label, font)
        {
            this.onClick = onClick;
        }

        public override void Update(MouseState mouse, KeyboardState keyboard)
        {
            Point mousePos = new Point(mouse.X, mouse.Y);
            bool hovered = InputRect.Contains(mousePos);

            if (hovered && mouse.LeftButton == ButtonState.Pressed)
                onClick?.Invoke();
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            var color = isHovered ? Color.Gray : Color.White;
            spriteBatch.Draw(texture, InputRect, color);
            spriteBatch.DrawString(font, label, new Vector2(InputRect.X + 10, InputRect.Y + 10), Color.Black);
        }
    }


}
