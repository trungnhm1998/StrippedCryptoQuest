using CryptoQuest.UI.Menu.ScriptableObjects;
using FSM;
using UnityEngine;

namespace CryptoQuest.UI.Battle.SelectCommand
{
    public class UISelectCommand : UIBattleMenu
    {
        public static readonly string SelectCommandState = "SelectCommand";

        public override string StateName => SelectCommandState;

        public override StateBase<string> CreateState(BattleMenuController controller)
        {
            return new SelectCommandState(controller.BattleMenuFSM);
        }
    }
}