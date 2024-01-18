using System;
using System.Collections.Generic;
using CryptoQuest.Battle.Events;
using CryptoQuest.UI.Dialogs.DialogWithCharacterName;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.Components.EnemyComponents.CustomBehaviours
{
    /// <summary>
    /// Enemy will show dialog at the start of each specific round
    /// </summary>
    [Serializable]
    public class TalkAtStartRound : BaseBehaviour
    {
        [Serializable]
        public struct TurnDialog
        {
            public int Turn;
            public LocalizedString[] DialogMessages;
        }

        [SerializeField] private TurnDialog[] _turnDialogs;
        private TinyMessageSubscriptionToken _startRoundToken;
        private Dictionary<int, LocalizedString[]> _turnDialogsMap = new();
        private UICharacterNamedDialog _dialog;
        private int _currentRound;

        public override void OnEnable(EnemyBehaviour enemyBehaviour)
        {
            base.OnEnable(enemyBehaviour);
            _startRoundToken = BattleEventBus.SubscribeEvent<RoundStartedEvent>(SetCurrentRound);
            _enemyBehaviour.TurnStarted += QueueDialog;

            foreach (var turnDialog in _turnDialogs)
            {
                _turnDialogsMap.TryAdd(turnDialog.Turn, turnDialog.DialogMessages);
            }

            DialogWithCharacterNameController.Instance.InstantiateAsync(SetDialog);
        }

        private void SetCurrentRound(RoundStartedEvent ctx)
        {
            _currentRound = ctx.Round;
        }


        private void SetDialog(UICharacterNamedDialog characterNamedDialog)
        {
            _dialog = characterNamedDialog;
            _dialog.Hide();
        }

        public override void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_startRoundToken);
            _enemyBehaviour.TurnStarted -= QueueDialog;
        }

        private void QueueDialog()
        {
            if (!_turnDialogsMap.TryGetValue(_currentRound, out var dialogs))
                return;

            foreach (var dialog in dialogs)
            {
                BattleEventBus.RaiseEvent<EnqueuePresentCommandEvent>(new EnqueuePresentCommandEvent(
                    new PresentDialogCommand(_dialog, dialog)));
            }
        }
    }
}
