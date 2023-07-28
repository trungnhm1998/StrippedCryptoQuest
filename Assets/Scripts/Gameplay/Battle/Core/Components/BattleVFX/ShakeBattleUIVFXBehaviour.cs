using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleVFX
{
    public class ShakeBattleUIVFXBehaviour : BattleVFXBehaviour
    {
        [Header("Listen Events")]
        [SerializeField] private VoidEventChannelSO _doneShakeBattleUIEventChannel;

        [Header("Raise Events")]
        [SerializeField] private VoidEventChannelSO _shakeBattleUIEventChannel;

        private void Start()
        {
            _shakeBattleUIEventChannel.RaiseEvent();
        }

        protected void OnEnable()
        {
            _doneShakeBattleUIEventChannel.EventRaised += FinishVFX;
        }

        protected void OnDisable()
        {
            _doneShakeBattleUIEventChannel.EventRaised -= FinishVFX;
        }
    }
}