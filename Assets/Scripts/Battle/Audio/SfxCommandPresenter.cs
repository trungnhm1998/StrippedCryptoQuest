using System.Collections;
using CryptoQuest.Audio.AudioData;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.Presenter.Commands;
using UnityEngine;

namespace CryptoQuest.Battle.Audio
{

    public class SfxCommandPresenter : MonoBehaviour
    {
        [SerializeField] private AudioCueEventChannelSO _sfxEventChannel;
        
        public void QueueSfxCommand(AudioCueSO cue)
        {
            var command = new SfxCommand(this, cue);
            BattleEventBus.RaiseEvent<EnqueuePresentCommandEvent>(
                new EnqueuePresentCommandEvent(command));
        }

        public void PlaySfx(AudioCueSO cue)
        {
            _sfxEventChannel.PlayAudio(cue);
        }
    }

    public class SfxCommand : IPresentCommand
    {
        private readonly SfxCommandPresenter _presenter;
        private AudioCueSO _sfxCue;

        public SfxCommand(SfxCommandPresenter presenter, AudioCueSO cue)
        {
            _presenter = presenter;
            _sfxCue = cue;
        }

        public IEnumerator Present()
        {
            _presenter.PlaySfx(_sfxCue);
            yield break;
        }
    }
}