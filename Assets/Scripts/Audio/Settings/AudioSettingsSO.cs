using CryptoQuest.Events;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Audio.Settings
{
    public class AudioSettingsSO : ScriptableObject
    {
        [SerializeField, HideInInspector]
        private float _volume = 1f;

        public event UnityAction<float> VolumeChanged;

        public float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                this.CallEventSafely(VolumeChanged, _volume);
            }
        }
    }
}