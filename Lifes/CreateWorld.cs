using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifes
{
    public enum TerrainType
    {
        Water,
        Plain,
        Forest,
        Mountain
    }
    public class CreateWorld
    {
        public int Width { get; }
        public int Height { get; }
        PerlinNoise perlinNoise;
        public TerrainType[,] TerrainMap { get; private set; }

        public static string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public CreateWorld(int width = 100, int height = 100, float scale = 0.5f, string seed = "")
        {
            Width = width;
            Height = height;
            if (seed == "")
            {
                seed = GenerateRandomString(32);
            }
            perlinNoise = new PerlinNoise(seed);
            TerrainMap = new TerrainType[Width, Height];

            // ★ここで地形を実際に生成
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float noise = perlinNoise.Noise(x * scale, y * scale);
                    if (noise < 0.35f)
                        TerrainMap[x, y] = TerrainType.Water;
                    else if (noise < 0.5f)
                        TerrainMap[x, y] = TerrainType.Plain;
                    else if (noise < 0.7f)
                        TerrainMap[x, y] = TerrainType.Forest;
                    else
                        TerrainMap[x, y] = TerrainType.Mountain;
                }
            }
        }

        public TerrainType GetTerrain(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                throw new ArgumentOutOfRangeException();
            return TerrainMap[x, y];
        }
    }
}