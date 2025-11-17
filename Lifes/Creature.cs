using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq; // (将来的に使うかもしれないので残しておきます)

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

    // --- プロパティ ---
    public Vector2 Position;
    public Color Color; // 基本色
    public Color Color2; // (★追加) アクセント色
    public float Size;
    public float Speed;
    public int Helth;
    public int MaxHelth;
    //public float MutationRate; // (現在DNAから設定されていませんが、将来的に拡張可能です)
    public byte[,] looks; // 見た目 (テクスチャデータ)
    public Codon[] Dna { get; private set; }
    public string[] parents;
    public string id; // 個体識別ID
    // 寿命系
    public float MaxAge { get; private set; }
    public float CurrentAge { get; private set; }
    public bool IsAlive { get; private set; }

    public Texture2D Texture; // (public に変更しました。Drawで使うため)

    // 乱数生成器 (一つに統一)
    private static Random _rand = new Random();

    // 突然変異率
    private const double MUTATION_RATE = 0.01;

    // ----------------------------------------------------
    // (1) 両親から新しいCreatureを生成するコンストラクタ
    // ----------------------------------------------------
    public Creature(Creature parent1, Creature parent2, GraphicsDevice device)
    {
        // --- 1. IDと親情報の生成 ---
        id = Guid.NewGuid().ToString(); // ユニークID
        parents = new string[2] { parent1.id, parent2.id };

        // --- 2. DNAの生成 (交叉・突然変異) ---
        int len1 = parent1.Dna.Length;
        int len2 = parent2.Dna.Length;

        if (len1 != len2)
        {
            Codon[] shorterDna = (len1 < len2) ? parent1.Dna : parent2.Dna;
            Codon[] longerDna = (len1 < len2) ? parent2.Dna : parent1.Dna;
            Codon[] adjustedLongerDna = new Codon[shorterDna.Length];
            Array.Copy(longerDna, adjustedLongerDna, shorterDna.Length);
            this.Dna = GenerateDna(shorterDna, adjustedLongerDna);
        }
        else
        {
            this.Dna = GenerateDna(parent1.Dna, parent2.Dna);
        }

        // --- 3. DNAの翻訳 (特性決定) ---
        DecodeDna(Dna);

        // --- 4. テクスチャの生成 ---
        // (DecodeDna で looks が決定した後に実行)
        this.Texture = GenerateDotTexture(looks, device);
    }

    // ----------------------------------------------------
    // (2) ランダムな初期個体を生成するコンストラクタ
    // ----------------------------------------------------
    public Creature(int initialDnaLength, GraphicsDevice device)
    {
        // --- 1. IDと親情報の生成 ---
        this.id = Guid.NewGuid().ToString(); // ユニークID
        this.parents = new string[2] { "ROOT", "ROOT" }; // 親なし(初期個体)

        // --- 2. ランダムなDNAの生成 ---
        this.Dna = new Codon[initialDnaLength];
        for (int i = 0; i < initialDnaLength; i++)
        {
            this.Dna[i] = new Codon(
                (Base)_rand.Next(4), // (★ rand から _rand に修正)
                (Base)_rand.Next(4),
                (Base)_rand.Next(4)
            );
        }

        // --- 3. DNAの翻訳 (特性決定) ---
        DecodeDna(Dna);

        // --- 4. テクスチャの生成 ---
        Texture = GenerateDotTexture(looks, device);
    }

    // ----------------------------------------------------
    // DNAの交叉と突然変異
    // ----------------------------------------------------
    private static Codon[] GenerateDna(Codon[] dna1, Codon[] dna2)
    {
        int dnaLength = dna1.Length;
        Codon[] newDna = new Codon[dnaLength];

        // (DNA長が 1 の場合 _rand.Next(1, 0) となりエラーになるため最小値を設定)
        int crossoverPoint = (dnaLength > 1) ? _rand.Next(1, dnaLength - 1) : 0;

        // ステップ1: 交叉
        for (int i = 0; i < dnaLength; i++)
        {
            newDna[i] = (i < crossoverPoint) ? dna1[i] : dna2[i];
        }

        // ステップ2: 突然変異
        for (int i = 0; i < newDna.Length; i++)
        {
            if (_rand.NextDouble() < MUTATION_RATE) newDna[i].B1 = (Base)_rand.Next(4);
            if (_rand.NextDouble() < MUTATION_RATE) newDna[i].B2 = (Base)_rand.Next(4);
            if (_rand.NextDouble() < MUTATION_RATE) newDna[i].B3 = (Base)_rand.Next(4);
        }

        return newDna;
    }


    // ----------------------------------------------------
    // DNAの翻訳 (特性デコード)
    // (★可変サイズ＆左右対称に対応)
    // ----------------------------------------------------
    private void DecodeDna(Codon[] dna)
    {
        // デフォルト値
        Color = Color.Gray;
        Color2 = Color.White; // (★アクセント色)
        Size = 8f;
        Speed = 1.0f;
        MaxHelth = 100;
        Helth = MaxHelth;
        Position = new Vector2(_rand.Next(800), _rand.Next(480));
        MaxAge = 1000f;
        CurrentAge = 0f;
        IsAlive = true;

        // 例1: 最初のコドン (dna[0]) で基本色
        if (dna.Length > 0)
        {
            byte r = (byte)((byte)dna[0].B1 * 64);
            byte g = (byte)((byte)dna[0].B2 * 64);
            byte b = (byte)((byte)dna[0].B3 * 64);
            Color = new Color(r, g, b);
        }

        // 例2: 2番目のコドン (dna[1]) でサイズ
        if (dna.Length > 1)
        {
            float sizeValue = (float)((byte)dna[1].B1 + (byte)dna[1].B2 + (byte)dna[1].B3);
            Size = 5.0f + sizeValue;
        }

        // 例3: 3番目のコドン (dna[2]) でスピード
        if (dna.Length > 2)
        {
            float speedValue = (float)((byte)dna[2].B1 + (byte)dna[2].B2 + (byte)dna[2].B3);
            Speed = 0.5f + (speedValue / 10.0f);
        }

        // 4番目のコドン (dna[3]) でアクセント色
        if (dna.Length > 3)
        {
            byte r = (byte)((byte)dna[3].B3 * 64); // 基本色とRGBの順番を変えてみる
            byte g = (byte)((byte)dna[3].B2 * 64);
            byte b = (byte)((byte)dna[3].B1 * 64);
            Color2 = new Color(r, g, b);
        }

        // 5番目のコドン (dna[4]) で寿命
        if (dna.Length > 4)
        {
            float lifeValue = (float)((byte)dna[4].B1 + (byte)dna[4].B2 + (byte)dna[4].B3);
            MaxAge = 5f + (lifeValue * 2f);
        }

        // 6番目のコドン (dna[5]) でテクスチャサイズを決定
        int width = 4;
        int height = 4;
        if (dna.Length > 4)
        {
            // B1 (0～3) を使って幅を 4～7 の範囲で決定
            width = 4 + (byte)dna[5].B1;
            // B2 (0～3) を使って高さを 4～7 の範囲で決定
            height = 4 + (byte)dna[5].B2;
        }

        this.looks = new byte[width, height]; // 可変サイズで配列を初期化

        // 7番目以降のコドンで見た目(looks)を左右対称に決定

        // 必要なコドン数 (左右対称なので幅の半分(切り上げ))
        int halfWidth = (int)Math.Ceiling(width / 2.0);
        int requiredCodonsForLooks = halfWidth * height;

        // ここで体力
        if(dna.Length > 5)
        {
            MaxHelth = ((byte)dna[6].B1 + 1)*((byte)dna[6].B2);
        }

        // looks の DNA はインデックス 6 から読み取る
        int looksDnaStartIndex = 7;

        if (dna.Length > looksDnaStartIndex + requiredCodonsForLooks)
        {
            for (int y = 0; y < height; y++)
            {
                // 左半分をDNAから決定
                for (int x = 0; x < halfWidth; x++)
                {
                    int codonIndex = looksDnaStartIndex + (y * halfWidth) + x;
                    // B1 (0～3) を looks の値として使う
                    this.looks[x, y] = (byte)dna[codonIndex].B1;
                }

                // 右半分を反転コピー (★左右対称)
                for (int x = halfWidth; x < width; x++)
                {
                    int mirroredX = width - 1 - x;
                    this.looks[x, y] = this.looks[mirroredX, y];
                }
            }
        }
        // (DNAが足りない場合は、new byte[,] で初期化された 0 (透明) のままになる)
    }

    private Texture2D GenerateDotTexture(byte[,] textures, GraphicsDevice device)
    {
        // (★ rand から _rand に修正)
        // (このメソッドはもうランダム性を使わないので、以下の2行は削除してもOK)
        // int w = _rand.Next(3, 8);
        // int h = _rand.Next(3, 8);

        // (★修正) 引数 textures のサイズをそのまま使う
        int width = textures.GetLength(0);
        int height = textures.GetLength(1);
        Color[] data = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;

                // (★修正) looks の値 (0～3) を取得
                byte lookValue = textures[x, y];

                // (★修正) 0.5 のランダムではなく、lookValue に応じて色を決定
                switch (lookValue)
                {
                    case 0:
                        data[index] = Color.Transparent; // 0 = 透明
                        break;
                    case 1:
                        data[index] = this.Color; // 1 = 基本色
                        break;
                    case 2:
                        data[index] = this.Color2; // 2 = アクセント色
                        break;
                    case 3:
                        // 3 = 影色 (基本色を暗くする)
                        data[index] = new Color(this.Color.R / 2, this.Color.G / 2, this.Color.B / 2);
                        break;
                    default:
                        data[index] = Color.Transparent;
                        break;
                }
            }
        }

        Texture2D tex = new Texture2D(device, width, height);
        tex.SetData(data);
        return tex;
    }

    // ----------------------------------------------------
    // 更新
    // ----------------------------------------------------
    public void Update(GameTime gameTime)
    {
        if (!IsAlive) return;

        CurrentAge += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if(CurrentAge >= MaxAge)
        {
            IsAlive = false;
            return;
        }

        // (★ rand から _rand に修正)
        Position.X += (float)(_rand.NextDouble() - 0.5) * Speed * 2;
        Position.Y += (float)(_rand.NextDouble() - 0.5) * Speed * 2;

        
    }

    // ----------------------------------------------------
    // 描画
    // ----------------------------------------------------
    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsAlive && Texture != null) // テクスチャがnullでないことを確認
        {
            // (Size / 5f はテクスチャサイズによって調整が必要かもしれません)
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, Size / 5f, SpriteEffects.None, 0f);
        }
    }
}