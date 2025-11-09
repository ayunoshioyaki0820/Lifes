using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lifes
{
    public enum GameState
    {
        Menu,
        Playing
    }
    public class GameManager : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private GameState currentState = GameState.Menu;
        private SpriteFont pixelFont, pixelFontTitle;

        private int selectedIndex = 0;
        private string[] menuItems = { "NewGame", "Continue", "Exit" };

        private MainGame game;
        Camera _camera;
        private KeyboardState _previousKeyboardState;

        public GameManager()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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

            game = new MainGame(GraphicsDevice);
            _camera = new Camera();
        }

        protected override void Update(GameTime gameTime)
        {
            var key = Keyboard.GetState();

            if (currentState == GameState.Menu)
            {
                if (key.IsKeyDown(Keys.Up) && _previousKeyboardState.IsKeyUp(Keys.Up))
                {
                    selectedIndex += -1;
                    if (selectedIndex < 0)
                        selectedIndex = menuItems.Length - 1;
                }
                if (key.IsKeyDown(Keys.Down) && _previousKeyboardState.IsKeyUp(Keys.Down))
                {
                    selectedIndex += 1;
                    if (selectedIndex >= menuItems.Length)
                        selectedIndex = 0;
                }

                if (key.IsKeyDown(Keys.Enter) && _previousKeyboardState.IsKeyUp(Keys.Enter))
                {
                    if (selectedIndex == 0)
                        currentState = GameState.Playing;
                    else if (selectedIndex == 2)
                        Exit();
                }
            }
            else if (currentState == GameState.Playing)
            {
                game.Update(gameTime);
                _camera.Update(gameTime);
                if (key.IsKeyDown(Keys.Escape))
                {
                    currentState = GameState.Menu;
                }
            }

            if (key.IsKeyDown(Keys.F11) && _previousKeyboardState.IsKeyUp(Keys.F11)){
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

            _previousKeyboardState = key;
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
                _spriteBatch.DrawString(pixelFontTitle, title, new Vector2((float)centerX - titleSize.X, centerY - 180), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

                for (int i = 0; i < menuItems.Length; i++)
                {
                    var text = (i == selectedIndex) ? ">" + menuItems[i] : menuItems[i];
                    var color = (i == selectedIndex) ? Color.Yellow : Color.Gray;
                    var size = pixelFont.MeasureString(text) * 2;
                    _spriteBatch.DrawString(pixelFont, text, new Vector2(centerX - size.X / 2, centerY + 20 + i * 50), color, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                }

                _spriteBatch.End();
            }
            else if (currentState == GameState.Playing)
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.GetViewMatrix(_graphics));
                // ゲーム画面描画
                game.Draw(_spriteBatch);

                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
