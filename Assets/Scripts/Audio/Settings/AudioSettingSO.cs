using System;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Audio.Settings
{
    [Serializable]
    public class AudioSettingSO : ScriptableObject
    {
        /// <summary>
        /// Serialized field for show in inspector custom volume
        /// Hide in inspector because we have custom editor UI for this
        /// </summary>
        [SerializeField, HideInInspector] private float _volume = .5f;

        public event UnityAction<float> VolumeChanged;

        public float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                VolumeChanged.SafeInvoke(_volume);
            }
        }
    }
}