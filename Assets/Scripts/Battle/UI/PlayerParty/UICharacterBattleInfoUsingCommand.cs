using System;
using System.Collections;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.Presenter.Commands;
using TinyMessenger;

namespace CryptoQuest.Battle.UI.PlayerParty
{
    public class UpdateCharacterInfoCommand : IPresentCommand
    {
        private float _newValue;
        private Action<float> _setValueCallback;

        public UpdateCharacterInfoCommand(Action<float> callback, float newValue)
        {
            _setValueCallback = callback;
            _newValue = newValue;
        }

        public IEnumerator Present()
        {
            _setValueCallback?.Invoke(_newValue);
            yield break;
        }
    }

    public class UICharacterBattleInfoUsingCommand : UICharacterBattleInfo
    {
        private bool _isStartedTurn = false;
        private TinyMessageSubscriptionToken _roundStartedEventToken;

        private void OnEnable()
        {
            _isStartedTurn = false;
            _roundStartedEventToken = BattleEventBus.SubscribeEvent<RoundStartedEvent>(SetStartedRound);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_roundStartedEventToken);
        }

        private void SetStartedRound(RoundStartedEvent eventObject)
        {
            _isStartedTurn = true;
        }

        public override void SetCurrentHp(float currentHp)
        {
            if (!_isStartedTurn)
            {
                base.SetCurrentHp(currentHp);
                return;
            }

            var command = new UpdateCharacterInfoCommand(base.SetCurrentHp, currentHp);
            BattleEventBus.RaiseEvent<EnqueuePresentCommandEvent>(
                new EnqueuePresentCommandEvent(command));
        }

        public override void SetCurrentMp(float currentMp)
        {
            if (!_isStartedTurn)
            {
                base.SetCurrentMp(currentMp);
                return;
            }

            var command = new UpdateCharacterInfoCommand(base.SetCurrentMp, currentMp);
            BattleEventBus.RaiseEvent<EnqueuePresentCommandEvent>(
                new EnqueuePresentCommandEvent(command));
        }
    }
}
