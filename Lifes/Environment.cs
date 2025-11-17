using System;

namespace Lifes
{
    public class Environment
    {
        // 基本パラメータ
        public float Temperature { get; set; }      // 温度
        public float Humidity { get; set; }         // 湿度
        public float FoodSupply { get; set; }       // 食料量
        public float WaterSupply { get; set; }      // 水資源
        public float Pollution { get; set; }        // 汚染度

        // コンストラクタ
        public Environment(float temperature = 20f, float humidity = 0.5f,
                           float foodSupply = 100f, float waterSupply = 100f, float pollution = 0f)
        {
            Temperature = temperature;
            Humidity = humidity;
            FoodSupply = foodSupply;
            WaterSupply = waterSupply;
            Pollution = pollution;
        }

        // 時間経過での更新（ターンやフレームごとに呼ぶ）
        public void UpdateEnvironment(float deltaTime)
        {
            // 例: 食料や水の自然減少
            FoodSupply = Math.Max(0, FoodSupply - deltaTime * 0.1f);
            WaterSupply = Math.Max(0, WaterSupply - deltaTime * 0.1f);

            // 例: 汚染は少しずつ減少
            Pollution = Math.Max(0, Pollution - deltaTime * 0.05f);

            // 他にも天候変化や季節変化をここで加えられる
        }

        // 環境の状態を簡易チェック
        public bool IsHabitable()
        {
            return Temperature >= -10 && Temperature <= 40 &&
                   Humidity >= 0.1f && Humidity <= 0.9f &&
                   FoodSupply > 0 && WaterSupply > 0;
        }

        public override string ToString()
        {
            return $"Temp: {Temperature}°C, Humidity: {Humidity * 100}%, Food: {FoodSupply}, Water: {WaterSupply}, Pollution: {Pollution}";
        }
    }

}
