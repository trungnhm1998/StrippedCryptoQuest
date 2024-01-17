using System.Collections;
using CryptoQuest.Audio.AudioData;
using CryptoQuest.Audio.AudioEmitters;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.Audio
{
    public class LoopBgmAfterPlaySfxHandler : MonoBehaviour
    {
        [Header("Raise on")] [SerializeField] private AudioCueEventChannelSO _musicEventChannel;
        [SerializeField] private AudioCueEventChannelSO _sfxEventChannel;

        private TinyMessageSubscriptionToken _sfxPlayedToken;
        private Coroutine _coroutine;

        private void OnDisable()
        {
            if (_sfxPlayedToken != null) ActionDispatcher.Unbind(_sfxPlayedToken);
        }

        public void StopAndPlayAudio(AudioCueSO sfx, AudioCueSO bgm)
        {
            StopPlayCoroutine();
            _coroutine = StartCoroutine(CoPlayAudio(sfx, bgm));
        }

        public void StopPlayCoroutine()
        {
            _musicEventChannel.StopAudio(null);
            if (_coroutine == null) return;
            OnDisable();
            StopCoroutine(_coroutine);
        }

        // Because intro bgm and main bgm need to connect so I wait a little faster
        // but the Audio emitter has that annoying bgm fade that I cant setup
        // TODO: separate fade as optional when playing bgm @longle
        private const float PURPOSE_OVERLAP_DELAY = 0.5f;
        private IEnumerator CoPlayAudio(AudioCueSO sfx, AudioCueSO bgm)
        {
            AudioEmitter introEmmiter = null;
            _sfxPlayedToken = ActionDispatcher.Bind<AudioPlayed>((AudioPlayed ctx)
                => introEmmiter = ctx.Emitter);
            _sfxEventChannel.PlayAudio(sfx);

            yield return new WaitUntil(() => introEmmiter != null);

            yield return new WaitForSeconds(introEmmiter.GetClip().length - PURPOSE_OVERLAP_DELAY);

            ActionDispatcher.Unbind(_sfxPlayedToken);
            _musicEventChannel.PlayAudio(bgm);
        }
    }
}