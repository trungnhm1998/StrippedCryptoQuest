using System;
using CryptoQuest.Input;
using CryptoQuest.System.CutsceneSystem.Events;
using Input;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutsceneSystem
{
    public class CutsceneTrigger : MonoBehaviour
    {
        public event Action FinishedCutscene;
        [SerializeField] private PlayableDirector _playableDirector;
        [SerializeField] private InputMediatorSO _inputMediatorSo;

        [Header("Listening to")]
        [SerializeField] private QuestCutsceneDef questCutsceneDef;

        [Header("Raise on")]
        [SerializeField] private PlayCutsceneEvent _playCutsceneEvent;

        private void OnEnable()
        {
            if (questCutsceneDef != null) questCutsceneDef.EventRaised += PlayCutscene;
        }

        private void OnDisable()
        {
            if (questCutsceneDef != null) questCutsceneDef.EventRaised -= PlayCutscene;
        }

        public void PlayCutscene()
        {
            _inputMediatorSo.DisableAllInput();
            _playCutsceneEvent.RaiseEvent(_playableDirector, this);
        }

        public void StopCutscene()
        {
            _inputMediatorSo.EnableMapGameplayInput();
            FinishedCutscene?.Invoke();
        }
    }
}