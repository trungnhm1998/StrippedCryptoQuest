using CryptoQuest.Audio.AudioData;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Audio
{
    public class PlayMusicInTitleSceneAction : ActionBase
    {
    }

    public class PlayMusicInTitleScene : SagaBase<PlayMusicInTitleSceneAction>
    {
        [Header("Raise on")] [SerializeField] private AudioCueEventChannelSO _musicEventChannel;

        [Header("Configs")] [SerializeField] private AudioCueSO _musicTrack;

        protected override void HandleAction(PlayMusicInTitleSceneAction ctx)
        {
            _musicEventChannel.PlayAudio(_musicTrack);
        }
    }
}