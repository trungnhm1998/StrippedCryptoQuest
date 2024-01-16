using CryptoQuest.Gameplay.Encounter;
using UnityEngine;

namespace CryptoQuest.Battle.Audio
{
    public class PreBattleAudioManager : MonoBehaviour
    {
        [SerializeField] private LoopBgmAfterPlaySfxHandler _audioHandler;

        public void PlayBattleMusic(Battlefield battlefield)
        {
            _audioHandler.StopAndPlayAudio(battlefield.AudioConfig.Intro,
                battlefield.AudioConfig.Bgm);
        }
    }
}