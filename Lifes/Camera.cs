using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Lifes
{

    public class Camera
    {
        public Vector2 Position { get; private set; } = Vector2.Zero;
        public float Zoom { get; private set; } = 1f;
        public float Rotation { get; private set; } = 0f;
        public float MoveSpeed { get; set; } = 5f;
        public float ZoomSpeed { get; set; } = 0.05f;

        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();

            // 矢印キーで移動
            if (keyboard.IsKeyDown(Keys.Right))
                Position += new Vector2(MoveSpeed, 0);
            if (keyboard.IsKeyDown(Keys.Left))
                Position += new Vector2(-MoveSpeed, 0);
            if (keyboard.IsKeyDown(Keys.Up))
                Position += new Vector2(0, -MoveSpeed);
            if (keyboard.IsKeyDown(Keys.Down))
                Position += new Vector2(0, MoveSpeed);

            // Q/Eで回転
            if (keyboard.IsKeyDown(Keys.Q))
                Rotation -= 0.02f;
            if (keyboard.IsKeyDown(Keys.E))
                Rotation += 0.02f;

            // + / - でズーム
            if (keyboard.IsKeyDown(Keys.OemPlus) || keyboard.IsKeyDown(Keys.Add))
                Zoom += ZoomSpeed;
            if (keyboard.IsKeyDown(Keys.OemMinus) || keyboard.IsKeyDown(Keys.Subtract))
                Zoom -= ZoomSpeed;

            // ズーム値の制限
            Zoom = MathHelper.Clamp(Zoom, 0.2f, 3f);
        }

        public Matrix GetViewMatrix(GraphicsDeviceManager graphics)
        {
            var vp = graphics.GraphicsDevice.Viewport;
            return
                Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom) *
                Matrix.CreateTranslation(new Vector3(vp.Width / 2f, vp.Height / 2f, 0));
        }
    }

}
