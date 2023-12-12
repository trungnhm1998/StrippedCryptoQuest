using CryptoQuest.Audio.AudioData;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.UI.Logs;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.Audio
{
    // Play bgm of last scene after unload battle
    public class PostBattleSfxManager : MonoBehaviour
    {
        [field: SerializeField] private AudioCueSO _winSfx;
        [field: SerializeField] private AudioCueSO _winBgm;
        [field: SerializeField] private AudioCueSO _retreatSfx;

        [Header("Raise on")] [SerializeField] private LoopBgmAfterPlaySfxHandler _audioHandler;
        [SerializeField] private AudioCueEventChannelSO _sfxEventChannel;

        private TinyMessageSubscriptionToken _wonToken;
        private TinyMessageSubscriptionToken _retreatToken;

        private void OnEnable()
        {
            _wonToken = BattleEventBus.SubscribeEvent<BattleWonEvent>(OnBattleWon);
            _retreatToken = BattleEventBus.SubscribeEvent<RetreatedEvent>(
                _ => _sfxEventChannel.PlayAudio(_retreatSfx));
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_wonToken);
            ActionDispatcher.Unbind(_retreatToken);
        }

        private void OnBattleWon(BattleWonEvent ctx)
        {
            if (ctx.Loots.Count <= 0)
            {
                ActionDispatcher.Dispatch(new PlayCachedBgmAction());
                _sfxEventChannel.PlayAudio(_winSfx);
                return;
            }
            _audioHandler.StopAndPlayAudio(_winSfx, _winBgm);
        }
    }
}