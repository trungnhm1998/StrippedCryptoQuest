using System.Collections;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    class EnemyAttackBehaviour : NormalAttack
    {
        [SerializeField] private VoidEventChannelSO _shakeEvent;
        [SerializeField] private VoidEventChannelSO _shakeCompleteEvent;
        private bool _hasShaken;
        protected override IEnumerator OnPreAttack(Character target)
        {
            _shakeCompleteEvent.EventRaised += UpdateShakenState;
            _shakeEvent.RaiseEvent();
            yield return new WaitUntil(() => _hasShaken);
            _shakeCompleteEvent.EventRaised -= UpdateShakenState;
        }

        private void UpdateShakenState()
        {
            _hasShaken = true;
        }
    }
}