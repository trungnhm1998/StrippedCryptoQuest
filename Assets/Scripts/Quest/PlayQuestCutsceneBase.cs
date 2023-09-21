using CryptoQuest.Quest.Authoring;
using CryptoQuest.System.CutsceneSystem;
using UnityEngine;

namespace CryptoQuest.Quest
{
    public class PlayQuestCutsceneBase : MonoBehaviour
    {
        [field: SerializeField] public Authoring.Quest Quest { get; protected set; }
        [field: SerializeField] public CutsceneTask Task { get; protected set; }
        [field: SerializeField] public CutsceneTrigger CutsceneTrigger { get; protected set; }

        protected void PlayCutscene()
        {
            if (Quest.IsCompleted) return;
            if (!Quest.CanCompleteTask(Task)) return;
            if (Quest.HasTaskCompleted(Task)) return;
            if (!Task.PlayOnLoaded) return;

            CutsceneTrigger.PlayCutscene();
            CutsceneTrigger.FinishedCutscene += CompleteTask;
        }

        private void CompleteTask()
        {
            CutsceneTrigger.FinishedCutscene -= CompleteTask;

            Quest.CompleteTask(Task);
        }
    }
}