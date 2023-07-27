using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Audio
{
    [CreateAssetMenu(menuName = "Crypto Quest/Audio/Audio Config", fileName = "AudioConfigurationSO")]
    public class AudioConfigurationSO : ScriptableObject
    {
        [SerializeField] private EPriorityLevel _priorityLevel = EPriorityLevel.Standard;

        [HideInInspector]
        public int Priority
        {
            get { return (int)_priorityLevel; }
            set { _priorityLevel = (EPriorityLevel)value; }
        }

        [Header("Sound properties")]
        public bool Mute = false;

        [Range(0f, 1f)] public float Volume = 1f;


        private enum EPriorityLevel
        {
            Highest = 0,
            High = 64,
            Standard = 128,
            Low = 194,
            VeryLow = 256,
        }

        public void ApplyTo(AudioSource audioSource)
        {
            audioSource.mute = this.Mute;
            audioSource.priority = this.Priority;
            audioSource.volume = this.Volume;
        }
    }
}