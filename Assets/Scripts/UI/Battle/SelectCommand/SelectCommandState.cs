using System;
using CryptoQuest.PushdownFSM;

namespace CryptoQuest.UI.Battle.SelectCommand
{
    public class SelectCommandState : IState
    {
        private UISelectCommand _selectCommandUI;

        public SelectCommandState(UISelectCommand uiSelectCommand) { }

        public void OnEnter() { }

        public void OnExit() { }
    }
}