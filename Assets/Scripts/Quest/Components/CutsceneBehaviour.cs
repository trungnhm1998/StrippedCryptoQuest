using UnityEngine.Events;

namespace CryptoQuest.Quest
{
    public class CutsceneBehaviour : StageBehaviourBase
    {
        public CutsceneTask CutsceneTaskStage;
        public UnityEvent PlayCutsceneEvent;
        public UnityEvent CutsceneFinishedEvent;

        public override void Execute()
        {
            PlayCutsceneEvent.Invoke();
        }

        public void CutsceneFinished()
        {
            CutsceneFinishedEvent.Invoke();
        }
    }
}