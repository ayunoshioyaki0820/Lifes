using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace Lifes
{
    public enum GameState
    {
        Menu,
        WorldBuilding,
        WorldSelection,
        Playing
    }
    public class GameManager : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private static GameState currentState;
        internal static GameState currentStateSetter { get => currentState; set => currentState = value; }
        public SpriteFont pixelFont, pixelFontTitle;
        private Texture2D Logo;
        private Rectangle LogoRect;
        private Vector2 LogoPosition;

        private int selectedIndex;
        private string[] menuItems;

        private MainGame game;
        public static CreateWorld world;
        private WorldBuildingView worldBuildingView;
        Camera _camera;
        public static KeyboardState previousKeyboardState;
        public static MouseState previousMouseState;

        private static Version version;
        private int fpsCount;
        private int fps;
        private double Sec;

        public GameManager()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            menuItems =
            [
                "New World",
                "Continue",
                "Exit"
            ];
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            pixelFont = Content.Load<SpriteFont>("PixelFont");
            pixelFontTitle = Content.Load<SpriteFont>("PixelFontTitle");
            Logo = Content.Load<Texture2D>("logo");


            using (var stream = TitleContainer.OpenStream("Content/Settings/version.txt"))
            using (var reader = new System.IO.StreamReader(stream))
            {
                var versionString = reader.ReadToEnd().Trim();
                version = new Version(versionString);
            }

            game = new MainGame(GraphicsDevice);
            worldBuildingView = new WorldBuildingView(GraphicsDevice, pixelFont, CreateWorld.GenerateRandomString(16));
            _camera = new Camera();
        }

        protected override void Update(GameTime gameTime)
        {
            var key = Keyboard.GetState();
            var mouse = Mouse.GetState();
            if (currentState == GameState.Menu)
            {
                if ((key.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up)))
                {
                    selectedIndex--;
                    if (selectedIndex < 0)
                        selectedIndex = menuItems.Length - 1;
                }
                if (key.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))
                {
                    selectedIndex++;
                    if (selectedIndex >= menuItems.Length)
                        selectedIndex = 0;
                }

                if (key.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
                {
                    if (selectedIndex == 0)
                        currentState = GameState.WorldBuilding;
                    else if (selectedIndex == 1)
                        currentState = GameState.WorldSelection;
                    else if (selectedIndex == 2)
                        Exit();
                }

                if(mouse.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    var mouseX = mouse.X;
                    var mouseY = mouse.Y;
                    var centerX = _graphics.PreferredBackBufferWidth / 2;
                    var centerY = _graphics.PreferredBackBufferHeight / 2;
                    for (int i = 0; i < menuItems.Length; i++)
                    {
                        var text = (i == selectedIndex) ? ">" + menuItems[i] : menuItems[i];
                        var size = pixelFont.MeasureString(text) * 2;
                        var itemX = centerX - size.X / 2;
                        var itemY = centerY + 20 + i * 50;
                        var itemWidth = size.X;
                        var itemHeight = size.Y;
                        if (mouseX >= itemX && mouseX <= itemX + itemWidth &&
                            mouseY >= itemY && mouseY <= itemY + itemHeight)
                        {
                            selectedIndex = i;
                            if (selectedIndex == 0)
                                currentState = GameState.WorldBuilding;
                            else if (selectedIndex == 1)
                                currentState = GameState.WorldSelection;
                            else if (selectedIndex == 2)
                                Exit();
                        }
                    }
                } else if(mouse.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    var mouseX = mouse.X;
                    var mouseY = mouse.Y;
                    var centerX = _graphics.PreferredBackBufferWidth / 2;
                    var centerY = _graphics.PreferredBackBufferHeight / 2;
                    for (int i = 0; i < menuItems.Length; i++)
                    {
                        var text = (i == selectedIndex) ? ">" + menuItems[i] : menuItems[i];
                        var size = pixelFont.MeasureString(text) * 2;
                        var itemX = centerX - size.X / 2;
                        var itemY = centerY + 20 + i * 50;
                        var itemWidth = size.X;
                        var itemHeight = size.Y;
                        if (mouseX >= itemX && mouseX <= itemX + itemWidth &&
                            mouseY >= itemY && mouseY <= itemY + itemHeight)
                        {
                            selectedIndex = i;
                        }
                    }
                } else if(previousMouseState.ScrollWheelValue - mouse.ScrollWheelValue < 0)
                {
                    selectedIndex--;
                    if (selectedIndex < 0)
                        selectedIndex = menuItems.Length - 1;
                }
                else if(previousMouseState.ScrollWheelValue - mouse.ScrollWheelValue > 0)
                {
                    selectedIndex++;
                    if (selectedIndex >= menuItems.Length)
                        selectedIndex = 0;
                }
            }
            if (currentState == GameState.WorldBuilding)
            {
                worldBuildingView.Update(gameTime, key, mouse);
                if (key.IsKeyDown(Keys.Escape))
                {
                    currentState = GameState.Menu;
                }
            }
            else if (currentState == GameState.WorldSelection)
            { }
            else if (currentState == GameState.Playing)
            {
                game.Update(gameTime);
                _camera.Update(gameTime);
                if (key.IsKeyDown(Keys.Escape))
                {
                    currentState = GameState.Menu;
                }
            }

            if (key.IsKeyDown(Keys.F11) && previousKeyboardState.IsKeyUp(Keys.F11)){
                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                if (_graphics.IsFullScreen)
                {
                    _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                }
                else
                {
                    _graphics.PreferredBackBufferWidth = 800;
                    _graphics.PreferredBackBufferHeight = 450;
                }
                _graphics.ApplyChanges();
                GraphicsDevice.Clear(Color.Black);
            }

            previousKeyboardState = key;
            previousMouseState = mouse;

            Sec += gameTime.ElapsedGameTime.TotalSeconds;
            if(Sec < 1)
                fpsCount++;
            else
            {
                fps = fpsCount;
                Sec = 0;
                fpsCount = 0;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black); // 背景色をクリア

            

            if (currentState == GameState.Menu)
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

                var centerX = _graphics.PreferredBackBufferWidth / 2;
                var centerY = _graphics.PreferredBackBufferHeight / 2;

                var title = "Lifes";
                var titleSize = pixelFontTitle.MeasureString(title);
                LogoPosition = new Vector2(centerX - Logo.Width /2, centerY - Logo.Height);
                _spriteBatch.Draw(Logo, LogoPosition, Color.White);

                for (int i = 0; i < menuItems.Length; i++)
                {
                    var text = (i == selectedIndex) ? ">" + menuItems[i] : menuItems[i];
                    var color = (i == selectedIndex) ? Color.Yellow : Color.Gray;
                    var size = pixelFont.MeasureString(text) * 2;
                    _spriteBatch.DrawString(pixelFont, text, new Vector2(centerX - size.X / 2, centerY + 20 + i * 50), color, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                }
                var versionText = $"Version: {version}";
                var versionSize = pixelFont.MeasureString(versionText);
                _spriteBatch.DrawString(pixelFont, versionText, new Vector2(_graphics.PreferredBackBufferWidth - versionSize.X - 10, _graphics.PreferredBackBufferHeight - versionSize.Y - 10), Color.White);
                _spriteBatch.End();
            } else if (currentState == GameState.WorldBuilding)
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                // ワールドビルディング画面描画
                worldBuildingView.Draw(_spriteBatch, pixelFont);
                _spriteBatch.End();
            }
            else if (currentState == GameState.WorldSelection)
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                // ワールドセレクション画面描画
                _spriteBatch.DrawString(pixelFont, "World Selection View (Not Implemented)", new Vector2(50, 50), Color.White);
                _spriteBatch.End();
            }
            else if (currentState == GameState.Playing)
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.GetViewMatrix(_graphics));
                // ゲーム画面描画
                game.Draw(_spriteBatch);
                _spriteBatch.End();
                _spriteBatch.Begin();
                _spriteBatch.DrawString(pixelFont, "Creatures:" +game.CreaturesCount.ToString(), new Vector2(0, 20), Color.White);
                _spriteBatch.End();
            }

            _spriteBatch.Begin();
            _spriteBatch.DrawString(pixelFont, $"FPS: {fps}", new Vector2(0,0), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
