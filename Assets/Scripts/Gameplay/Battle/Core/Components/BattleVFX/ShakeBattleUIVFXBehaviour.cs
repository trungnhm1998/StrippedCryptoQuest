using IndiGames.Core.Events.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleVFX
{
    public class ShakeBattleUIVFXBehaviour : BattleVFXBehaviour
    {
        [Header("Listen Events")]
        [SerializeField] private VoidEventChannelSO _doneShakeBattleUIEventChannel;

        [Header("Raise Events")]
        [SerializeField] private VoidEventChannelSO _shakeBattleUIEventChannel;

        public override void Init(BattleActionDataSO data)
        {
            base.Init(data);
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