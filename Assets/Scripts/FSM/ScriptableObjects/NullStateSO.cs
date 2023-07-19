using CryptoQuest.FSM.ScriptableObjects.Base;
using UnityEngine;

namespace CryptoQuest.FSM.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NullStateSO", menuName = "Gameplay/Battle/FSM/States/Null State")]
    public class NullStateSO : BaseStateSO
    {
        public override void Execute(BaseStateMachine stateMachine) {}
        public override void OnEnterState(BaseStateMachine stateMachine) {}
        public override void OnExitState(BaseStateMachine stateMachine) {}
    }
}