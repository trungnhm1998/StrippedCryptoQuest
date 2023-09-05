using CryptoQuest.Gameplay.Encounter;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects
{
    public class MockBattleBus : BattleBus
    {
        public EnemyParty MockParty;

        public override EnemyParty CurrentEnemyParty => MockParty;
    }
}