using CryptoQuest.Gameplay.Encounter;
using UnityEngine;

namespace CryptoQuest.Battle.Audio
{
    public class PreBattleAudioManager : MonoBehaviour
    {
        [SerializeField] private LoopBgmAfterPlaySfxHandler _audioHandler;

        private void OnEnable()
        {
            BattleLoader.LoadBattle += PlayBattleMusic;
        }

        private void OnDisable()
        {
            BattleLoader.LoadBattle -= PlayBattleMusic;
        }

        private void PlayBattleMusic(Battlefield battlefield)
        {
            _audioHandler.StopAndPlayAudio(battlefield.AudioConfig.Intro,
                battlefield.AudioConfig.Bgm);
        }
    }
}