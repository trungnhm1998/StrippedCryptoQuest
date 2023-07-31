using UnityEngine;

namespace CryptoQuest.Audio.AudioData
{
    [CreateAssetMenu(menuName = "Crypto Quest/Audio/SFX Cue", fileName = "SFXCueSO")]
    public class SFXCueSO : AudioCueSO 
    {
        public SFXCueSO()
        {
            Looping = false;
        }
    }
}