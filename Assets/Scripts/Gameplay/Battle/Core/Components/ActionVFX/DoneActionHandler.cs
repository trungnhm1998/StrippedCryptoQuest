using CryptoQuest.GameHandler;
using System;

namespace CryptoQuest.Gameplay.Battle.Core.Components.ActionVFX
{
    public class DoneActionHandler : GameHandler<object>
    {
        public Action DoneAction;
        private ActionVFXController _actionVFXHandler;

        public DoneActionHandler(ActionVFXController handler)
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