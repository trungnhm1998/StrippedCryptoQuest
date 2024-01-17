using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Encounter;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.Audio
{
    public class PreBattleAudioManager : MonoBehaviour
    {
        [SerializeField] private LoopBgmAfterPlaySfxHandler _audioHandler;

        private TinyMessageSubscriptionToken _endBattleToken;

        private void OnEnable()
        {
            _endBattleToken = BattleEventBus.SubscribeEvent<UnloadingEvent>(_ 
                => _audioHandler.StopPlayCoroutine());
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_endBattleToken);
        }

        public void PlayBattleMusic(Battlefield battlefield)
        {
            _audioHandler.StopAndPlayAudio(battlefield.AudioConfig.Intro,
                battlefield.AudioConfig.Bgm);
        }
    }
}