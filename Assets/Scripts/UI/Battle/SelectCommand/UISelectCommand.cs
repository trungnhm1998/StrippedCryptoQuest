using CryptoQuest.UI.Battle.StateMachine;
using FSM;
using UnityEngine;

namespace CryptoQuest.UI.Battle.SelectCommand
{
    public class UISelectCommand : MonoBehaviour, IBattleMenu
    {
        public static readonly string SelectCommandState = "SelectCommand";

        public string StateName => SelectCommandState;

        public StateBase<string> CreateState(BattleMenuStateMachine machine)
        {
            return new SelectCommandState(machine);
        }
    }
}