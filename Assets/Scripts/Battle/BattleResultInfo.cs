using CryptoQuest.Gameplay.Encounter;

namespace CryptoQuest.Battle
{
    public class BattleResultInfo
    {
        public bool IsWin { get; set; }
        public Battlefield Battlefield { get; set; }
        public CompletedContext BattleContext { get; set; }
    }
}