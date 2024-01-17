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
        [SerializeField] private ResultSO _result;
        [field: SerializeField] private AudioCueSO _winSfx;
        [field: SerializeField] private AudioCueSO _winBgm;
        [field: SerializeField] private AudioCueSO _retreatSfx;

        [Header("Raise on")] [SerializeField] private LoopBgmAfterPlaySfxHandler _audioHandler;
        [SerializeField] private AudioCueEventChannelSO _sfxEventChannel;

        private TinyMessageSubscriptionToken _wonToken;

        private void OnEnable()
        {
            _wonToken = BattleEventBus.SubscribeEvent<UnloadingEvent>(_ =>
            {
                if (_result.State == ResultSO.EState.Retreat)
                {
                    _sfxEventChannel.PlayAudio(_retreatSfx);
                }
                else if (_result.State == ResultSO.EState.Win)
                {
                    PlayBattleWonSfx();
                }
            });
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_wonToken);
        }

        private void PlayBattleWonSfx()
        {
            if (_result.Loots.Count <= 0)
            {
                ActionDispatcher.Dispatch(new PlayCachedBgmAction());
                _sfxEventChannel.PlayAudio(_winSfx);
                return;
            }
            _audioHandler.StopAndPlayAudio(_winSfx, _winBgm);
        }
    }
}