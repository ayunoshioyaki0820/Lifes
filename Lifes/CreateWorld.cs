using System;
using System.Collections.Generic;
using System.Linq;

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

        public int Octaves { get; set; } = 4;
        public float Persistence { get; set; } = 0.5f;
        public float Lacunarity { get; set; } = 2.0f;

        private Perlin perlin;
        public TerrainType[,] TerrainMap { get; private set; }

        private static Random random = new Random();

        public static string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public CreateWorld(int width = 100, int height = 100, float scale = 0.05f, string seed = "")
        {
            Width = width;
            Height = height;

            if (string.IsNullOrEmpty(seed))
            {
                seed = GenerateRandomString(16);
                Console.WriteLine($"Generated Seed: {seed}");
            }

            // 文字列シード → 数値化
            int numericSeed = seed.GetHashCode();
            perlin = new Perlin(numericSeed);

            TerrainMap = new TerrainType[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float totalNoise = 0;
                    float amplitude = 1.0f;
                    float frequency = scale;
                    float maxAmplitude = 0;

                    for (int i = 0; i < Octaves; i++)
                    {
                        float noiseValue = perlin.Noise(x * frequency, y * frequency);
                        totalNoise += (noiseValue - 0.5f) * amplitude;
                        maxAmplitude += amplitude;

                        amplitude *= Persistence;
                        frequency *= Lacunarity;
                    }

                    float normalizedNoise = (totalNoise / maxAmplitude) + 0.5f;

                    if (normalizedNoise < 0.35f)
                        TerrainMap[x, y] = TerrainType.Water;
                    else if (normalizedNoise < 0.5f)
                        TerrainMap[x, y] = TerrainType.Plain;
                    else if (normalizedNoise < 0.7f)
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