using System;

namespace CryptoQuest.Gameplay.Character.LevelSystem
{
    public interface IAccumulatedExpCalculator
    {
        double CalculateAccumulatedExp(int level);
    }

    public class AccumulatedExpCalculator : IAccumulatedExpCalculator
    {
        public double CalculateAccumulatedExp(int level)
        {
            // So many magic number... but I cant define all of it
            double beforeRound = (LevelConstValue.BASE_VALUE * Math.Pow(level - 1.0, 0.9 + LevelConstValue.INCREASE_A_VALUE / 250.0) * level * (level + 1.0))
                / (6.0 + Math.Pow(level, 2.0)/50.0/LevelConstValue.INCREASE_B_VALUE) + (level - 1.0) * LevelConstValue.CORRECTION_VALUE;
            return Math.Round(beforeRound);
        }
    }

    public static class LevelConstValue
    {
        public const float BASE_VALUE = 30f;
        public const float CORRECTION_VALUE = 10f;
        public const float INCREASE_A_VALUE = 100f;
        public const float INCREASE_B_VALUE = 20f;
    }
}