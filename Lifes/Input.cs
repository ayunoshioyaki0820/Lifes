using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
#nullable enable

namespace Lifes
{
    internal abstract class InputBase
    {
        internal Rectangle InputRect;
        protected Rectangle[] border;
        internal Vector2 LabelPoint;
        internal string label;
        protected bool isActive;
        protected bool isHovered;
        protected float time;
        protected SpriteFont font;
        internal int fontHeight;
        internal int Width, Height;
        internal int Top, Bottom, Left, Right;

        public InputBase(Rectangle BaseInputRect, string label, SpriteFont font)
        {
            this.label = label;
            this.font = font;
            fontHeight = font.LineSpacing;
            InputRect = new Rectangle(BaseInputRect.X, BaseInputRect.Y + fontHeight, BaseInputRect.Width, BaseInputRect.Height);
            InitializeLayout(BaseInputRect);
            border =
            [
                new Rectangle(Left, Top, Width, 2),
                new Rectangle(Left, Bottom - 2, Width, 2),
                new Rectangle(Left, Top, 2, Height),
                new Rectangle(Right - 2, Top, 2, Height)
            ];
        }
        internal virtual void InitializeLayout(Rectangle rect)
        {
            Width = InputRect.Width;
            Height = InputRect.Height;
            Top = InputRect.Top;
            Bottom = InputRect.Bottom;
            Left = InputRect.Left;
            Right = InputRect.Right;

            border =
            [
                new Rectangle(Left, Top, Width, 2),
                new Rectangle(Left, Bottom - 2, Width, 2),
                new Rectangle(Left, Top, 2, Height),
                new Rectangle(Right - 2, Top, 2, Height)
            ];
        }

        public virtual void Update(MouseState mouse, KeyboardState keyboard, GameTime time)
        {
            Point mousePos = new(mouse.X, mouse.Y);
            isHovered = InputRect.Contains(mousePos);
            if (isHovered && mouse.LeftButton == ButtonState.Pressed)
                isActive = true;
            else if (mouse.LeftButton == ButtonState.Pressed && !isHovered)
                isActive = false;
            this.time += (float)time.ElapsedGameTime.TotalSeconds;
            if (this.time >= 1f)
                this.time = 0f;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            for (var i = 0; i < border.Length; i++)
                spriteBatch.Draw(texture, border[i], Color.DarkGray);
        }
    }

    internal class TextInput : InputBase
    {
        private string value = "";
        private Rectangle cursor;
        private KeyboardState _key;
        private KeyboardState _previousKey;
        private Vector2 valuePoint;
        private bool isShift = false;

        void CharInput(char c)
        {
            value += c;
        }
        void Input()
        {
            if (_key != _previousKey)
            {
                if (_key.IsKeyDown(Keys.LeftShift) || _key.IsKeyDown(Keys.RightShift))
                {
                    isShift = true;
                }
                else
                {
                    isShift = false;
                }

                foreach (var k in _key.GetPressedKeys())
                {
                    if (!_previousKey.IsKeyDown(k))
                    {
                        if (k == Keys.Back && value.Length > 0)
                        {
                            value = value[..^1];
                        }
                        else
                        {
                            var keyString = k.ToString();

                            if (keyString.Length == 1)
                            {
                                if (isShift)
                                    CharInput(char.ToUpper(keyString[0]));
                                else
                                    CharInput(char.ToLower(keyString[0]));
                            }
                            else if (keyString.StartsWith('D') && keyString.Length == 2)
                            {
                                if (isShift)
                                    CharInput(Data.NumbersShift[keyString[1].ToString()]);
                                else
                                {
                                    CharInput(keyString[1]);
                                }
                            }
                            else if (keyString.StartsWith("NumPad") && keyString.Length == 7)
                            {
                                CharInput(keyString[6]);
                            }
                            else if (keyString.StartsWith("Oem"))
                            {
                                if (isShift)
                                    CharInput(Data.OemShift[keyString]);
                                else
                                    CharInput(Data.OemNoShift[keyString]);
                            }
                            else if (k == Keys.Space)
                            {
                                CharInput(' ');
                            }
                            else if (k == Keys.Enter)
                            {
                                isActive = false;
                            }
                        }
                    }
                }
            }
        }

        public TextInput(Rectangle rect, string label, SpriteFont font) : base(rect, label, font) { }

        internal override void InitializeLayout(Rectangle rect)
        {
            base.InitializeLayout(rect);
            LabelPoint = new Vector2(rect.X, rect.Y);
            cursor = new Rectangle(InputRect.X + 5, InputRect.Y + (InputRect.Height - fontHeight) / 2, 2, fontHeight);
            valuePoint = new Vector2(cursor.X, cursor.Y + 3);
        }

        public override void Update(MouseState mouse, KeyboardState keyboard, GameTime g)
        {
            base.Update(mouse, keyboard, g);

            if (isActive)
            {
                Input();
            }

            _previousKey = _key;
            _key = keyboard;
            if (value.Length > 0)
                cursor.X = (int)(valuePoint.X + font.MeasureString(value).X) + 2;
            else
                cursor.X = (int)valuePoint.X;
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            var color = isHovered ? Color.WhiteSmoke : Color.White;
            spriteBatch.Draw(texture, InputRect, color);
            if (isActive && time < 0.75f)
            {
                spriteBatch.Draw(texture, cursor, Color.Black);
            }

            base.Draw(spriteBatch, texture);


            spriteBatch.DrawString(font, value, valuePoint, Color.Black);
            spriteBatch.DrawString(font, label, LabelPoint, Color.White);
        }
    }

    internal class NumberInput : InputBase
    {
        private int value = 0;
        private string valueString = "";
        private bool isMinus;
        private Rectangle cursor;
        private Vector2 valuePoint;
        private float timeSinceLastChange = 0f;

        public NumberInput(Rectangle rect, string label,SpriteFont font, bool isMinus = false) : base(rect, label, font) {
            this.isMinus = isMinus;
        }
        internal override void InitializeLayout(Rectangle rect)
        {
            base.InitializeLayout(rect);
            LabelPoint = new Vector2(rect.X, rect.Y);
            cursor = new Rectangle(InputRect.X + 5, InputRect.Y + (InputRect.Height - fontHeight) / 2, 2, fontHeight);
            valuePoint = new Vector2(cursor.X, cursor.Y + 3);
        }

        public override void Update(MouseState mouse, KeyboardState keyboard, GameTime g)
        {
            base.Update(mouse, keyboard, g);
            valueString = value.ToString();
            if (isActive)
            {
                if(GameManager.previousKeyboardState != keyboard || timeSinceLastChange > 1)
                {
                    if (keyboard.IsKeyDown(Keys.Up)) value++;
                    else if (keyboard.IsKeyDown(Keys.Down)) value--;
                    else if(keyboard.GetPressedKeyCount() > 0)
                    {
                        foreach(var k in keyboard.GetPressedKeys())
                        {
                            if (k == Keys.Back)
                            {
                                if (valueString.Length > 0)
                                {
                                    valueString = valueString[..^1];
                                    if (valueString.Length > 0)
                                        value = int.Parse(valueString);
                                    else
                                        value = 0;
                                }
                            }
                            else if(k.ToString()[0] == 'D')
                                {
                                var numChar = k.ToString()[1];
                                if (char.IsDigit(numChar))
                                {
                                    valueString += numChar;
                                    value = int.Parse(valueString);
                                }
                            }
                            Console.WriteLine(k);
                            Console.WriteLine(valueString);
                        }
                    }
                    timeSinceLastChange += (float)g.ElapsedGameTime.TotalSeconds;
                }
                if(GameManager.previousKeyboardState == keyboard && keyboard.GetPressedKeyCount() > 0)
                {
                    timeSinceLastChange += (float)g.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    timeSinceLastChange = 0f;
                }
            }
            if(!isMinus && value < 0) value = 0;
            if (valueString.Length > 0)
                cursor.X = (int)(valuePoint.X + font.MeasureString(valueString).X) + 2;
            else
                cursor.X = (int)valuePoint.X;
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            var color = isHovered ? Color.WhiteSmoke : Color.White;
            spriteBatch.Draw(texture, InputRect, color);
            if (isActive && time < 0.75f)
            {
                spriteBatch.Draw(texture, cursor, Color.Black);
            }

            base.Draw(spriteBatch, texture);
            spriteBatch.DrawString(font, valueString, valuePoint, Color.Black);
            spriteBatch.DrawString(font, label, LabelPoint, Color.White);
            
        }
    }

    internal class ButtonInput : InputBase
    {
        private Action? onClick;
        bool isPressedNow;
        bool wasPressed;
        public ButtonInput(Rectangle rect, string label, SpriteFont font, Action? onClick = null,  bool isTextCenter = true) : base(rect, label, font)
        {
            this.onClick = onClick;
            if (isTextCenter)
                LabelPoint = new Vector2(InputRect.X + InputRect.Width / 2 - (font.MeasureString(label).X)/2, InputRect.Y + (InputRect.Height - fontHeight) /2 +3);
            else
                LabelPoint = new Vector2(InputRect.X + 10, InputRect.Y + 10);

            if (onClick == null)
            {
                this.onClick = () => {
                    Console.WriteLine($"Button {label} clicked.");
                };
            }
        }
        internal override void InitializeLayout(Rectangle rect)
        {
            InputRect.Y -= fontHeight;
            base.InitializeLayout(rect);
        }

        public override void Update(MouseState mouse, KeyboardState keyboard, GameTime g)
        {
            Point mousePos = new(mouse.X, mouse.Y);
            isHovered = InputRect.Contains(mousePos);
            isPressedNow = mouse.LeftButton == ButtonState.Pressed;
            wasPressed = GameManager.previousMouseState.LeftButton == ButtonState.Pressed;
            if (isHovered && isPressedNow && !wasPressed)
            {
                onClick?.Invoke();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            var color = isHovered? isPressedNow? Color.Gray: Color.WhiteSmoke : Color.White;
            spriteBatch.Draw(texture, InputRect, color);
            spriteBatch.DrawString(font, label, LabelPoint, Color.Black);
            base.Draw(spriteBatch, texture);
        }
    }


}
