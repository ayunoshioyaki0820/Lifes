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
        public TerrainType[,] TerrainMap { get; private set; }

        public CreateWorld(int width = 100, int height = 100, float scale = 0.07f)
        {
            Width = width;
            Height = height;
            TerrainMap = new TerrainType[width, height];
            GenerateWorld(scale);
        }

        private void GenerateWorld(float scale)
        {
            var rand = new Random();
            float seed = (float)rand.NextDouble() * 1000f;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float nx = x * scale + seed;
                    float ny = y * scale + seed;
                    float noise = SimplePerlin.Noise(nx, ny);

                    // ノイズ値によって地形を決定
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

        // 任意の座標のTerrainTypeを取得
        public TerrainType GetTerrain(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                throw new ArgumentOutOfRangeException();
            return TerrainMap[x, y];
        }
    }

    // 非常に簡易なPerlinノイズ風サンプル（本格的なものはNuGetやGitHubで入手可）
    public static class SimplePerlin
    {
        public static float Noise(float x, float y)
        {
            return (float)(Math.Sin(x * 2.3 + y * 7.1) + Math.Cos(x * 5.13 - y * 1.73)) * 0.5f + 0.5f;
        }
    }
}
