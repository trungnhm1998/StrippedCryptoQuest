using CryptoQuest.FSM;
using CryptoQuest.FSM.ScriptableObjects.Base;
using CryptoQuest.Gameplay.Battle.Core.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.BattleFSM.Actions
{
    [CreateAssetMenu(fileName = "BattleActionSO", menuName = "Gameplay/Battle/FSM/Actions/Battle Action")]
    public class BattleActionSO : FSMActionSO
    {
        private BattleManager _manager;
        public BattleManager Manager => _manager;

        public override void Execute(BaseStateMachine stateMachine)
        {
            if (_manager != null) return;
            _manager = stateMachine.GetComponent<BattleManager>();
        }
    }
}