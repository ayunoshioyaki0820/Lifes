using System;

public class Perlin
{
    private int[] p; // 512要素のルックアップテーブル

    public Perlin(int seed)
    {
        // シードから乱数を作る
        Random rand = new Random(seed);

        int[] permutation = new int[256];
        for (int i = 0; i < 256; i++)
            permutation[i] = i;

        // シャッフル
        for (int i = 0; i < 256; i++)
        {
            int swapIndex = rand.Next(256);
            (permutation[i], permutation[swapIndex]) = (permutation[swapIndex], permutation[i]);
        }

        // 512要素に拡張
        p = new int[512];
        for (int i = 0; i < 512; i++)
            p[i] = permutation[i % 256];
    }

    public float Noise(float x, float y)
    {
        int X = (int)MathF.Floor(x) & 255;
        int Y = (int)MathF.Floor(y) & 255;

        x -= MathF.Floor(x);
        y -= MathF.Floor(y);

        float u = Fade(x);
        float v = Fade(y);

        int A = (p[X] + Y) & 255;
        int B = (p[X + 1] + Y) & 255;

        float result = Lerp(v,
            Lerp(u, Grad(p[A], x, y), Grad(p[B], x - 1, y)),
            Lerp(u, Grad(p[A + 1], x, y - 1), Grad(p[B + 1], x - 1, y - 1))
        );

        return (result + 1f) / 2f;
    }

    private static float Fade(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    private static float Lerp(float t, float a, float b)
    {
        return a + t * (b - a);
    }

    private static float Grad(int hash, float x, float y)
    {
        int h = hash & 3;
        float u = h < 2 ? x : y;
        float v = h < 2 ? y : x;
        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }
}
