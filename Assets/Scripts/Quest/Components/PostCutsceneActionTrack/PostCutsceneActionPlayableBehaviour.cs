using System;
using CryptoQuest.Quest.Actions;
using CryptoQuest.Quest.Controllers;
using CryptoQuest.System.CutsceneSystem;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.Quest.Components.PostCutsceneActionTrack
{
    [Serializable]
    public class PostCutsceneActionPlayableBehaviour : PlayableBehaviour
    {
        public BattleAction _action;
        private bool _played = false;

        ~PostCutsceneActionPlayableBehaviour()
        {
            CutsceneManager.CutsceneCompleted -= OnCutsceneFinished;
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (_played)
                return;
            _played = true;

            if (Application.isPlaying)
            {
                if (!playable.GetGraph().IsPlaying()) return;
                CutsceneManager.CutsceneCompleted += OnCutsceneFinished;
            }
        }

        private void OnCutsceneFinished()
        {
            CutsceneManager.CutsceneCompleted -= OnCutsceneFinished;
            //TODO: turn off player input when transition in case player spam ESC
            QuestCutsceneController.OnTriggerNextAction?.Invoke(_action);
        }
    }
}