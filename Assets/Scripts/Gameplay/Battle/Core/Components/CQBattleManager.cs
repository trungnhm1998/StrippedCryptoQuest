using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components
{
    public class CQBattleManager : BattleManager
    {
        [SerializeField] private GameplayBus _gameplayBus;

        private void OnValidate()
        {
            // To make sure that planner can only set team 2 as opponent and team 1 is reserved for player team 
            if (BattleTeam1 == null) return;
            BattleTeam2 = BattleTeam1;
            BattleTeam1 = null;
        }

        protected override void StartBattle()
        {
            BattleTeam1 = _gameplayBus.PlayerTeam;
            base.StartBattle();
        }
    }   
}