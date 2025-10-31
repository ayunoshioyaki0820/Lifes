using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

class Creature
{
    public enum Base : byte
    {
        A = 0,
        T = 1,
        G = 2,
        C = 3
    }
    public struct Codon
    {
        public Base B1;
        public Base B2;
        public Base B3;

        public Codon(Base b1, Base b2, Base b3)
        {
            this.B1 = b1;
            this.B2 = b2;
            this.B3 = b3;
        }

    }

    public Vector2 Position;
    public Color Color;
    public float Size;
    public float Speed;
    public float MutationRate;
    public byte[,] looks;
    public Codon[] Dna { get; private set; }
    public string[] parents;
    public string id;

    private Texture2D texture;

    // 乱数生成器 (MonoGameのGameクラスなどで一つだけ生成し、コンストラクタで渡すのが望ましい)
    private static Random _rand = new Random();

    // 突然変異率 (例: 1% の確率で塩基が変化する)
    private const double MUTATION_RATE = 0.01;

    // 両親から新しいCreatureを生成するコンストラクタ
    // Creatureのコンストラクタ
    public Creature(Creature parent1, Creature parent2, GraphicsDevice device)
    {
        // 親のDNA長が異なる場合の安全策 (短い方に合わせる)
        int len1 = parent1.Dna.Length;
        int len2 = parent2.Dna.Length;

        if (len1 != len2)
        {
            // DNA長が異なる場合の交叉は少し工夫が必要
            // ここでは単純化のため、短い方のDNA配列を基準にします
            Codon[] shorterDna = (len1 < len2) ? parent1.Dna : parent2.Dna;
            Codon[] longerDna = (len1 < len2) ? parent2.Dna : parent1.Dna;

            // 短い方の長さに合わせて切り詰めた配列を渡す
            Codon[] adjustedLongerDna = new Codon[shorterDna.Length];
            Array.Copy(longerDna, adjustedLongerDna, shorterDna.Length);

            this.Dna = GenerateDna(shorterDna, adjustedLongerDna);
        }
        else
        {
            // 長さが同じ場合
            this.Dna = GenerateDna(parent1.Dna, parent2.Dna);
        }

        DecodeDna(Dna);
        parents = new string[2] { parent1.id, parent2.id };
        Texture = GenerateDotTexture(looks, device);
    }

    // (DNAをランダムに生成する初期個体用のコンストラクタも必要)
    public Creature(int initialDnaLength, GraphicsDevice device)
    {
        this.Dna = new Codon[initialDnaLength];
        for (int i = 0; i < initialDnaLength; i++)
        {
            this.Dna[i] = new Codon(
                (Base)rand.Next(4),
                (Base)rand.Next(4),
                (Base)rand.Next(4)
            );
        }

        DecodeDna(Dna);
        Texture = GenerateDotTexture(looks, device);
    }

    private static Codon[] GenerateDna(Codon[] dna1, Codon[] dna2)
    {
        // DNAの長さ (両親で同じと仮定。異なる場合は短い方に合わせるなどルールが必要)
        int dnaLength = dna1.Length;
        Codon[] newDna = new Codon[dnaLength];
        int crossoverPoint = _rand.Next(1, dnaLength - 1);

        for (int i = 0; i < dnaLength; i++)
        {
            if (i < crossoverPoint)
            {
                newDna[i] = dna1[i];
            }
            else
            {
                newDna[i] = dna2[i];
            }
        }
        for (int i = 0; i < newDna.Length; i++)
        {
            // B1 塩基の突然変異
            if (_rand.NextDouble() < MUTATION_RATE)
            {
                newDna[i].B1 = (Base)_rand.Next(4); // 0～3 (A,T,G,C) のランダムな値
            }

            // B2 塩基の突然変異
            if (_rand.NextDouble() < MUTATION_RATE)
            {
                newDna[i].B2 = (Base)_rand.Next(4);
            }

            // B3 塩基の突然変異
            if (_rand.NextDouble() < MUTATION_RATE)
            {
                newDna[i].B3 = (Base)_rand.Next(4);
            }
        }

        return newDna;
    }


    public Texture2D Texture;

    private static Random rand = new Random();

    private Texture2D GenerateDotTexture(byte[,] tecstures ,GraphicsDevice device)
    {
        Color[] data = new Color[tecstures.Length];

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = rand.NextDouble() > 0.5 ? Color : Color.Transparent;
        }

        Texture2D tex = new Texture2D(device, tecstures.GetLength(0), tecstures.GetLength(1));
        tex.SetData(data);
        return tex;
    }

    // DNAを翻訳し、生物の特性（見た目、能力）を決定するメソッド
    // (引数として dna を受け取るように変更)
    private void DecodeDna(Codon[] dna)
    {
        // --- これはあくまで「設計例」です ---

        // デフォルト値の設定（DNAが短すぎる場合など）
        this.Color = Color.Gray;
        this.Size = 8f;
        this.Speed = 1.0f;
        this.Position = new Vector2(_rand.Next(800), _rand.Next(600)); // _rand は統一済みのRandomインスタンス

        // 例1: 最初のコドンで基本色を決める
        if (dna.Length > 0)
        {
            // 3塩基(0～3)の値をR,G,Bに割り当て (0～192の範囲)
            byte r = (byte)((byte)dna[0].B1 * 64);
            byte g = (byte)((byte)dna[0].B2 * 64);
            byte b = (byte)((byte)dna[0].B3 * 64);
            this.Color = new Color(r, g, b);
        }

        // 例2: 2番目のコドンでサイズを決める
        if (dna.Length > 1)
        {
            // 3塩基の合計値 (0～9) をサイズに反映
            float sizeValue = (float)((byte)dna[1].B1 + (byte)dna[1].B2 + (byte)dna[1].B3);
            this.Size = 5.0f + sizeValue; // 5.0～14.0 のサイズ
        }

        // 例3: 3番目のコドンでスピードを決める
        if (dna.Length > 2)
        {
            float speedValue = (float)((byte)dna[2].B1 + (byte)dna[2].B2 + (byte)dna[2].B3);
            this.Speed = 0.5f + (speedValue / 10.0f); // 0.5～1.4 のスピード
        }

        // 4-20番目のコドンで見た目を決める
        if (dna.Length > 19)
        {
            byte[,] textureData = new byte[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    textureData[i,j] = (byte)dna[4*i + j].B1;
                }
            }
            this.looks = textureData;
        }
    }

    public void Update()
    {
        Position.X += (float)(rand.NextDouble() - 0.5) * Speed * 2;
        Position.Y += (float)(rand.NextDouble() - 0.5) * Speed * 2;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        
        spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, Size / 5f, SpriteEffects.None, 0f);
    }
}
