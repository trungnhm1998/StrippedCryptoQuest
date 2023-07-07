using UnityEngine;
using CryptoQuest.FSM;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "BattleStateSO", menuName = "Gameplay/Battle/FSM/States/Battle State")]
    public class BattleStateSO : StateSO
    {
        public BattleManager BattleManager { get; protected set; }

        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            var manager = stateMachine.GetComponent<BattleManager>();
            if (manager == null) return;
            
            BattleManager = manager;
        }
    }
}