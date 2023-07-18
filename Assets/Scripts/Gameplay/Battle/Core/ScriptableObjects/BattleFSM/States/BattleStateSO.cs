using CryptoQuest.FSM;
using CryptoQuest.FSM.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.Core.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.BattleFSM.States
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