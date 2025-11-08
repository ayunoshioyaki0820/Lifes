using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lifes
{
    public enum TutorialSteps
    {
        None,
        Move,
        Edit,
        Done
    }
    public class MainGame
    {
        private List<Creature> creatures = new List<Creature>();
        private List<Food> foods = new List<Food>();
        private double evolveTimer = 0;
        private GraphicsDevice graphicsDevice;
        public SpriteFont pixelFont;

        public CreateWorld world = new CreateWorld();
        Texture2D pixel;
        const int tileSize = 32;

        Color GetColorByTerrain(TerrainType t)
        {
            switch (t)
            {
                case TerrainType.Water: return Color.Blue;
                case TerrainType.Plain: return Color.LightGreen;
                case TerrainType.Forest: return Color.DarkGreen;
                case TerrainType.Mountain: return Color.SaddleBrown;
                default: return Color.Black;
            }
        }

        public MainGame(GraphicsDevice device)
        {
            graphicsDevice = device;
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            for (int i = 0; i < 10; i++)
                creatures.Add(new Creature(20, graphicsDevice));
            for (int i = 0; i < 5; i++)
                foods.Add(new Food("Apple", 50, 30, new byte[,] {
                    { 0,1,1,0 },
                    { 1,1,1,1 },
                    { 1,1,1,1 },
                    { 0,1,1,0 }
                }, graphicsDevice));
        }
        private float testCountes = 0f;

        public void Update(GameTime gameTime)
        {
            foreach (var c in creatures)
            {
                c.Update(gameTime);
                if (!c.IsAlive)
                {
                    creatures.Remove(c);
                    break;
                }
            }
            

            foreach (var f in foods)
                f.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            evolveTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (evolveTimer > 5)
            {
                creatures.Add(new Creature(creatures[0], creatures[1], graphicsDevice));
                evolveTimer = 0;
            }
            // デバッグ情報の表示
            testCountes += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (testCountes > 1f)
            {
                Console.WriteLine($"Creatures: {creatures.Count}, Foods: {foods.Count}");

                // 位置情報を文字列でまとめる
                var positions = creatures
                    .Select(c => $"({(int)c.Position.X}, {(int)c.Position.Y})")
                    .ToList();

                Console.WriteLine("Creature positions: " + string.Join(", ", positions));

                testCountes = 0f;
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var c in creatures)
                c.Draw(spriteBatch);

            foreach (var f in foods)
                f.Draw(spriteBatch);



            for (int x = 0; x < world.Width; x++)
                for (int y = 0; y < world.Height; y++)
                {
                    TerrainType type = world.GetTerrain(x, y);
                    Color c = GetColorByTerrain(type);
                    Rectangle rect = new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);
                    spriteBatch.Draw(pixel, rect, c);
                }
        }
    }

}
