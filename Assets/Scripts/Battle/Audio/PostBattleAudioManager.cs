using System.Linq;
using CryptoQuest.Audio.AudioData;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.Audio
{
    public class PlayCachedBgmAction : ActionBase { }

    // Play bgm of last scene after unload battle
    public class PostBattleAudioManager : SagaBase<PlayCachedBgmAction>
    {
        [SerializeField] private AudioCueSO[] _ignoredCues;
        [SerializeField] private GameStateSO _gameState;
        [Header("Raise on")] [SerializeField] private AudioCueEventChannelSO _musicEventChannel;

        private AudioCueSO _lastAudioCue;

        private TinyMessageSubscriptionToken _retreatToken;
        private TinyMessageSubscriptionToken _loseToken;

        protected override void OnEnable()
        {
            base.OnEnable();
            _musicEventChannel.AudioPlayRequested += CacheLastAudio;
            _retreatToken = BattleEventBus.SubscribeEvent<BattleRetreatedEvent>(_ => PlayLastCachedAudio());
            _loseToken = BattleEventBus.SubscribeEvent<BattleLostEvent>(_ => PlayLastCachedAudio());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _musicEventChannel.AudioPlayRequested -= CacheLastAudio;
            ActionDispatcher.Unbind(_retreatToken);
            ActionDispatcher.Unbind(_loseToken);
        }

        private void CacheLastAudio(AudioCueSO cue)
        {
            if (_gameState.CurrentGameState == EGameState.Battle || _ignoredCues.Contains(cue)) return;
            _lastAudioCue = cue;
        }

        private void PlayLastCachedAudio()
        {
            _musicEventChannel.PlayAudio(_lastAudioCue);
        }

        protected override void HandleAction(PlayCachedBgmAction ctx)
        {
            PlayLastCachedAudio();
        }
    }
}