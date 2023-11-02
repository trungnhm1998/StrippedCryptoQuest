using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.System.CutsceneSystem
{
    public class CutsceneChoiceController : MonoBehaviour
    {
        public static Action<string> MadeChoice;

        [SerializeField] private List<string> _choiceMadeIds = new();

        private void OnEnable()
        {
            MadeChoice += OnChoiceMade;
            CutSceneChoiceInfo.ConfigureChoice += ConfigureChoiceInfo;
            CutsceneManager.CutsceneCompleted += ClearCutsceneChoiceInfo;
        }

        private void OnDisable()
        {
            MadeChoice -= OnChoiceMade;
            CutSceneChoiceInfo.ConfigureChoice -= ConfigureChoiceInfo;
            CutsceneManager.CutsceneCompleted -= ClearCutsceneChoiceInfo;
        }

        private void OnChoiceMade(string choiceId)
        {
            _choiceMadeIds.Add(choiceId);
        }

        private bool HasMadeChoice(string choiceId)
        {
            return _choiceMadeIds.Contains(choiceId);
        }

        private void ConfigureChoiceInfo(CutSceneChoiceInfo choiceInfo)
        {
            choiceInfo.HasMadeChoice = HasMadeChoice(choiceInfo.ChoiceId);
        }

        private void ClearCutsceneChoiceInfo()
        {
            _choiceMadeIds.Clear();
        }
    }

    public class CutSceneChoiceInfo
    {
        public static Action<CutSceneChoiceInfo> ConfigureChoice;
        public string ChoiceId;
        public bool HasMadeChoice;

        public CutSceneChoiceInfo(string choiceId)
        {
            ChoiceId = choiceId;
            HasMadeChoice = false;
        }

        public void ConfigureChoiceStatus()
        {
            ConfigureChoice?.Invoke(this);
        }
    }
}