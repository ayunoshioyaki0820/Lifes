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

        public CreateWorld(int width = 100, int height = 100, float scale = 0.2f, string seed = "")
        {
            Width = width;
            Height = height;
            if (seed == "")
            {
                seed = GenerateRandomString(32);
            }
            perlinNoise = new PerlinNoise(seed);
            TerrainMap = new TerrainType[Width, Height];
        }
    }
}
