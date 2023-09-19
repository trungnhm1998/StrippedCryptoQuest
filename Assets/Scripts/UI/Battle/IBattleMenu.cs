using CryptoQuest.PushdownFSM;
using CryptoQuest.UI.Battle.StateMachine;
using FSM;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public interface IBattleMenu
    {
        string StateName { get; }
        
        /// <summary>
        /// Act as a factory method to create the state for each panel.
        /// </summary>
        StateBase<string> CreateState(BattleMenuStateMachine machine);
    }
}