using CryptoQuest.GameHandler;
using System;

namespace CryptoQuest.Gameplay.Battle.Core.Components.ActionVFXHandler
{
    public class DoneActionHandler : GameHandler<object>
    {
        public Action DoneAction;
        private ActionVFXHandler _actionVFXHandler;

        public DoneActionHandler(ActionVFXHandler handler)
        {
            _actionVFXHandler = handler;
        }

        public override void Handle()
        {
            DoneAction?.Invoke();
            _actionVFXHandler.ResetHandler();
        }
    }
}