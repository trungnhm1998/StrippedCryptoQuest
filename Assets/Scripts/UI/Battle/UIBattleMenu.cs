using CryptoQuest.UI.Menu.ScriptableObjects;
using FSM;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public interface IBattleMenu
    {
        string StateName { get; }
        StateBase<string> CreateState(BattleMenuController controller);
    }

    public abstract class UIBattleMenu : MonoBehaviour, IBattleMenu
    {
        public abstract string StateName { get; }

        /// <summary>
        /// Act as a factory method to create the state for each panel.
        /// </summary>
        public abstract StateBase<string> CreateState(BattleMenuController controller);
    }
}