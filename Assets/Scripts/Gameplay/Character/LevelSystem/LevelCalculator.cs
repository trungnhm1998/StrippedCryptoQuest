using UnityEngine;

namespace CryptoQuest.Gameplay.Character.LevelSystem
{
    public interface ILevelCalculator
    {
        int[] RequiredExps { get; }
        int CalculateCurrentLevel(int currentExp);

    }

    public class LevelCalculator : ILevelCalculator
    {
        private int _maxLevel;
        public int[] RequiredExps { get; set; }

        public LevelCalculator(int maxLevel)
        {
            _maxLevel = maxLevel;
            InitRequiredExps();
        }

        private void InitRequiredExps()
        {
            RequiredExps = new int[_maxLevel];
            RequiredExps[0] = 0;

            var accumulatedExps = CalculateAccumulatedExps();

            for (int i = 1; i < _maxLevel; i++)
            {
                RequiredExps[i] = accumulatedExps[i] - accumulatedExps[i - 1];
            }
        }

        private int[] CalculateAccumulatedExps()
        {
            var accumulatedExps = new int[_maxLevel];
            accumulatedExps[0] = 0;
            
            var calculator = new AccumulatedExpCalculator();
            for (int i = 1; i < _maxLevel; i++)
            {
                accumulatedExps[i] = (int) calculator.CalculateAccumulatedExp(i + 1);
            }
            return accumulatedExps;
        }

        public int CalculateCurrentLevel(int currentExp)
        {
            for (int i = 0; i < RequiredExps.Length; i++)
            {
                if (currentExp >= RequiredExps[i]) continue;
                return i;
            }
            // Return max level if currentExp excess
            return _maxLevel;
        }
    }
}
