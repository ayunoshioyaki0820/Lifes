using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifes
{
    public class PerlinNoise
    {
        private int seed;
        private Random rand;
        static int CreateSeed(string seed)
        {
            unchecked
            {
                int hash = 17;
                foreach ( char c in seed )
                {
                    hash = hash * 31 + c;
                }
                return hash;
            }
        }
        public PerlinNoise(string seed)
        {
            this.seed = CreateSeed(seed);
            rand = new Random(this.seed);
        }
        public float Noise(float x, float y)
        {
            int xi = (int)x;
            int yi = (int)y;
            float xf = x - xi;
            float yf = y - yi;

            float v00 = Value(xi, yi);
            float v10 = Value(xi + 1, yi);
            float v01 = Value(xi, yi + 1);
            float v11 = Value(xi + 1, yi + 1);

            float i1 = Lerp(v00, v10, xf);
            float i2 = Lerp(v01, v11, xf);
            return Lerp(i1, i2, yf);
        }

        private float Value(int x, int y)
        {
            // グリッド座標+seedで毎回同じ値
            int combined = x * 4967 + y * 3251 + seed;
            var localRand = new Random(combined);
            return (float)localRand.NextDouble();
        }

        private float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }
    }
}
