using System.Collections;
using CryptoQuest.Audio.AudioData;
using CryptoQuest.Audio.AudioEmitters;
using CryptoQuest.Gameplay.Encounter;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public class PreBattleAudioManager : MonoBehaviour
    {
        [Header("Raise on")] [SerializeField] private AudioCueEventChannelSO _musicEventChannel;
        [SerializeField] private AudioCueEventChannelSO _sfxEventChannel;

        private TinyMessageSubscriptionToken _sfxPlayedToken;

        private void OnEnable()
        {
            BattleLoader.LoadBattle += PlayBattleMusic;
        }

        private void OnDisable()
        {
            BattleLoader.LoadBattle -= PlayBattleMusic;
            if (_sfxPlayedToken != null) ActionDispatcher.Unbind(_sfxPlayedToken);
        }

        private void PlayBattleMusic(Battlefield battlefield)
        {
            _musicEventChannel.StopAudio(null);
            StartCoroutine(CoPlayMusic(battlefield));
        }

        // Because intro bgm and main bgm need to connect so I wait a little faster
        // but the Audio emitter has that annoying bgm fade that I cant setup
        // TODO: separate fade as optional when playing bgm @longle
        private const float PURPOSE_OVERLAP_DELAY = 0.5f;
        private IEnumerator CoPlayMusic(Battlefield battlefield)
        {
            AudioEmitter introEmmiter = null;
            _sfxPlayedToken = ActionDispatcher.Bind<AudioPlayed>((AudioPlayed ctx)
                => introEmmiter = ctx.Emitter);
            _sfxEventChannel.PlayAudio(battlefield.AudioConfig.Intro);

            yield return new WaitUntil(() => introEmmiter != null);

            yield return new WaitForSeconds(introEmmiter.GetClip().length - PURPOSE_OVERLAP_DELAY);

            ActionDispatcher.Unbind(_sfxPlayedToken);
            _musicEventChannel.PlayAudio(battlefield.AudioConfig.Bgm);
        }
    }
}