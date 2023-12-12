using System.Collections;
using CryptoQuest.AbilitySystem;
using CryptoQuest.Audio.AudioData;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.Presenter.Commands;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Battle.Audio
{
    public class DeadSfxPlayer : MonoBehaviour
    {
        [SerializeField] private UnityEvent<AudioCueSO> _presentSfxEvent;
        [SerializeField] private AudioCueSO _deadSfx;
        
        private TinyMessageSubscriptionToken _effectAddedToken;

        private void OnEnable()
        {
            _effectAddedToken = BattleEventBus.SubscribeEvent<EffectAddedEvent>(OnEffectAdded);
        } 

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_effectAddedToken);
        }

        private void OnEffectAdded(EffectAddedEvent ctx)
        {
            if (ctx.Tag != TagsDef.Dead) return;
            _presentSfxEvent.Invoke(_deadSfx);
        }
    }
}