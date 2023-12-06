using CryptoQuest.Audio.AudioData;
using CryptoQuest.Battle.Events;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle
{
    // Play bgm of last scene after unload battle
    public class PostBattleAudioManager : MonoBehaviour
    {
        [SerializeField] private BattleBus _battleBus;
        [Header("Raise on")] [SerializeField] private AudioCueEventChannelSO _musicEventChannel;

        private AudioCueSO _lastAudioCue;

        private TinyMessageSubscriptionToken _unloadBattle;

        private void OnEnable()
        {
            _musicEventChannel.AudioPlayRequested += CacheLastAudio;
            _unloadBattle = BattleEventBus.SubscribeEvent<UnloadingEvent>((ctx) => PlayLastCachedAudio(ctx));
        }

        private void OnDisable()
        {
            _musicEventChannel.AudioPlayRequested -= CacheLastAudio;
            ActionDispatcher.Unbind(_unloadBattle);
        }

        private void CacheLastAudio(AudioCueSO clue)
        {
            if (_battleBus.CurrentBattlefield != null && 
                clue == _battleBus.CurrentBattlefield.AudioConfig.Bgm) return;
            _lastAudioCue = clue;
        }

        private void PlayLastCachedAudio(UnloadingEvent ctx)
        {
            _musicEventChannel.PlayAudio(_lastAudioCue);
        }
    }
}