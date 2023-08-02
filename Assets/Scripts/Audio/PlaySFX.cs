using CryptoQuest.Audio.AudioData;
using UnityEngine;

namespace CryptoQuest.Audio
{
    public class PlaySFX : MonoBehaviour
    {
        [Header("Raise on")] [SerializeField] private AudioCueEventChannelSO _sfxEventChannel;

        [Header("Configs")] public SFXCueSO SfxTrack;
        [SerializeField] private bool _playOnStart = default;

        private void OnEnable()
        {
            if (!_playOnStart) return;
            OnPlaySFX();
        }

        private void OnDisable()
        {
            _playOnStart = false;
            OnStopAudio();
        }

        public void OnPlaySFX()
        {
            _sfxEventChannel.PlayAudio(SfxTrack);
        }

        public void OnStopAudio()
        {
            _sfxEventChannel.StopAudio(SfxTrack);
        }
    }
}