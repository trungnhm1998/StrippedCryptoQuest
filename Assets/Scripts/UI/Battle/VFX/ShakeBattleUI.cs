using CryptoQuest.UI.Common;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Battle.VFX
{
    public class ShakeBattleUI : MonoBehaviour
    {
        [SerializeField] private UIShakeEffect _shakeEffect;

        [Header("Raise Events")]
        [SerializeField] private VoidEventChannelSO _doneShakeBattleUIEventChannel;

        [Header("Listen Events")]
        [SerializeField] private VoidEventChannelSO _shakeBattleUIEventChannel;

        protected void OnEnable()
        {
            _shakeBattleUIEventChannel.EventRaised += ShakeUI;
            _shakeEffect.ShakeComplete += DoneShakeUI;
        }

        protected void OnDisable()
        {
            _shakeBattleUIEventChannel.EventRaised -= ShakeUI;
            _shakeEffect.ShakeComplete -= DoneShakeUI;
        }

        private void ShakeUI()
        {
            _shakeEffect.Shake();
        }

        private void DoneShakeUI()
        {
            _doneShakeBattleUIEventChannel.RaiseEvent();
        }
    }
}