using System;
using CryptoQuest.Audio.AudioData;
using UnityEngine;

namespace CryptoQuest.Audio
{
    public class PlaySFX : MonoBehaviour
    {
        [Header("Raise on")] [SerializeField] private AudioCueEventChannelSO _sfxEventChannel;

        [Header("Configs")] [SerializeField] private AudioCueSO _sfxTrack;
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

        private void OnPlaySFX()
        {
            _sfxEventChannel.PlayAudio(_sfxTrack);
        }

        private void OnStopAudio()
        {
            _sfxEventChannel.StopAudio(_sfxTrack);
        }
    }
}