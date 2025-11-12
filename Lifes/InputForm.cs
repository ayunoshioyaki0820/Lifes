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

    public class InputForm
    {
        public String text = "";
        String BaseText = "Click to write...";
        bool focused = false;
        Rectangle formRect;
        MouseState _mouse;
        MouseState _previousMouse;
        KeyboardState _key;
        KeyboardState _previousKey;

        SpriteFont font;

        Vector2 textPos;
        Vector2 cursorPos;
        float textWidth;

        float _time;
        internal InputForm(Rectangle form, SpriteFont font, int limit = 32)
        {
            text = "";
            formRect = form;
            textPos = new Vector2(formRect.X + 8, formRect.Y + (formRect.Height - font.MeasureString(text).Y) / 4);
            this.font = font;
        }
        void charInput(char c)
        {
            text += c;
        }
        void Input()
        {
            if (_key != _previousKey)
            {
                foreach (var k in _key.GetPressedKeys())
                {
                    if (!_previousKey.IsKeyDown(k))
                    {
                        if (k == Keys.Back && text.Length > 0)
                        {
                            text = text[..^1];
                        }
                        else
                        {
                            var keyString = k.ToString().ToLower();
                            if (keyString.Length == 1)
                            {
                                charInput(keyString[0]);
                            }
                            else if (keyString.StartsWith("D") && keyString.Length == 2)
                            {
                                charInput(keyString[1]);
                            }
                            else if (keyString.StartsWith("NumPad") && keyString.Length == 7)
                            {
                                charInput(keyString[6]);
                            }
                            else if (k == Keys.Space)
                            {
                                charInput(' ');
                            }
                        }
                    }
                }
            }
        }
        void CheckFocus()
        {
            if (_mouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
            {
                if (formRect.Contains(_mouse.Position)) focused = true;
                else focused = false;
            }
            if (focused)
            {
                Input();
            }
        }
        internal void Update(KeyboardState key, MouseState mouse, GameTime gameTime)
        {
            _previousKey = _key;
            _previousMouse = _mouse;
            _key = key;
            _mouse = mouse;
            CheckFocus();

            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_time >= 1f)
            {
                _time = 0f;
            }
            textWidth = font.MeasureString(text).X;
            cursorPos = new Vector2(textPos.X + textWidth + 2, textPos.Y);
        }
        internal void Draw(SpriteBatch spriteBatch, Texture2D texture, Color borderColor)
        {
            // Draw text box background
            spriteBatch.Draw(texture, formRect, Color.LightGray);


            //// 枠線（上・下・左・右）
            spriteBatch.Draw(texture, new Rectangle(formRect.Left, formRect.Top, formRect.Width, 2), borderColor);
            spriteBatch.Draw(texture, new Rectangle(formRect.Left, formRect.Bottom - 2, formRect.Width, 2), borderColor);
            spriteBatch.Draw(texture, new Rectangle(formRect.Left, formRect.Top, 2, formRect.Height), borderColor);
            spriteBatch.Draw(texture, new Rectangle(formRect.Right - 2, formRect.Top, 2, formRect.Height), borderColor);
            // Draw text
            if (text.Length == 0 && !focused)
            {
                spriteBatch.DrawString(font, BaseText, textPos, Color.DarkGray);
            }
            else
            {
                spriteBatch.DrawString(font, text, textPos, Color.Black);
            }
            if (focused && _time < 0.5f)
            {
                spriteBatch.DrawString(font, "|", cursorPos, Color.Black);
            }
        }
    }
}
