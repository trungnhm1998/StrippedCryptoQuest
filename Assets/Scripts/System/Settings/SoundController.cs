using CryptoQuest.Events.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest
{
    public class SoundController : MonoBehaviour
    {
        [Header("Listen on")]
        [SerializeField] private SoundValueSO _soundValueEventChannel;

        public void OnSoundChanged(float value)
        {
            _soundValueEventChannel.Value = value;
        }
    }
}