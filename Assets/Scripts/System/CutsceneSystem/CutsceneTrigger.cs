using System;
using CryptoQuest.System.CutsceneSystem.Events;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutsceneSystem
{
    public class CutsceneTrigger : MonoBehaviour
    {
        public event Action FinishedCutscene;
        [SerializeField] private PlayableDirector _playableDirector;

        [Header("Listening to")]
        [SerializeField] private QuestCutsceneDef questCutsceneDef;
        
        [Header("Raise on")]
        [SerializeField] private PlayCutsceneEvent _playCutsceneEvent;

        private void OnEnable()
        {
           questCutsceneDef.EventRaised += PlayCutscene; 
        }

        private void OnDisable()
        {
           questCutsceneDef.EventRaised -= PlayCutscene; 
        }

        public void PlayCutscene()
        {
            _playCutsceneEvent.RaiseEvent(_playableDirector, this);
        }

        public void StopCutscene()
        {
            FinishedCutscene?.Invoke();
        }
    }
}