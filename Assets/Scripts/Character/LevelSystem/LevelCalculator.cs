namespace CryptoQuest.Character.LevelSystem
{
    public interface ILevelCalculator
    {
        int[] RequiredExps { get; }
        int[] AccumulatedExps { get; }
        int CalculateCurrentLevel(float currentExp);
    }

    public class LevelCalculator : ILevelCalculator
    {
        private int _maxLevel;
        public int[] AccumulatedExps { get; set; }
        public int[] RequiredExps { get; set; }

        public LevelCalculator(int maxLevel)
        {
            _maxLevel = maxLevel;
            if (_maxLevel <= 0) return;
            CalculateAccumulatedExps();
            InitRequiredExps();
        }

        private void InitRequiredExps()
        {
            RequiredExps = new int[_maxLevel];
            RequiredExps[0] = 0;

            for (int i = 1; i < _maxLevel; i++)
            {
                RequiredExps[i] = AccumulatedExps[i] - AccumulatedExps[i - 1];
            }
        }

        private void CalculateAccumulatedExps()
        {
            AccumulatedExps = new int[_maxLevel];
            AccumulatedExps[0] = 0;
            
            var calculator = new AccumulatedExpCalculator();
            for (int i = 1; i < _maxLevel; i++)
            {
                AccumulatedExps[i] = (int) calculator.CalculateAccumulatedExp(i + 1);
            }
        }

        public int CalculateCurrentLevel(float currentExp)
        {
            for (int i = 0; i < AccumulatedExps.Length; i++)
            {
                if (currentExp >= AccumulatedExps[i]) continue;
                return i;
            }
            // Return max level if currentExp excess
            return _maxLevel;
        }
    }
}
