using CryptoQuest.Battle.Components;

namespace CryptoQuest.Battle.Commands
{
    public class RetreatCommand : ICommand
    {
        private readonly RetreatBehaviour _retreatBehaviour;
        private readonly float _highestEnemySpeed;

        public RetreatCommand(HeroBehaviour hero, float highestEnemySpeed)
        {
            _retreatBehaviour = hero.GetComponent<RetreatBehaviour>();
            _highestEnemySpeed = highestEnemySpeed;
        }

        public void Execute() => _retreatBehaviour.Retreat(_highestEnemySpeed);
    }
}