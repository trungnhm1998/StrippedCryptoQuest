using CryptoQuest.Audio;

namespace CryptoQuest.System.SaveSystem.Actions
{
    public class SaveAudioAction : SaveActionBase<AudioManager>
    {
        public SaveAudioAction(AudioManager obj) : base(obj)
        {
        }
    }

    public class LoadAudioAction : SaveActionBase<AudioManager>
    {
        public LoadAudioAction(AudioManager obj) : base(obj)
        {
        }
    }

    public class SaveAudioCompletedAction : SaveCompletedActionBase
    {
        public SaveAudioCompletedAction(bool result) : base(result)
        {
        }
    }

    public class LoadAudioCompletedAction : SaveCompletedActionBase
    {
        public LoadAudioCompletedAction(bool result) : base(result)
        {
        }
    }
}