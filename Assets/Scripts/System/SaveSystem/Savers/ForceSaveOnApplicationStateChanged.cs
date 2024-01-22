using System;
using CryptoQuest.Bridge;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Events
{
    public class ForceSaveOnApplicationStateChanged : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _forceSaveEvent;
        [SerializeField] private VoidEventChannelSO _saveFinishedEventChannel;
        [SerializeField, Tooltip("Seconds")] private float _saveInterval = 10f;

        private void Start()
        {
            ApplicationEventHandler.RegisterOnBeforeUnloadEventCallback(gameObject.name, nameof(OnBeforeUnloaded));
            ApplicationEventHandler.RegisterOnFocusChangedEventCallback(gameObject.name, nameof(OnFocusChanged));
        }

        private void OnEnable()
        {
            _saveFinishedEventChannel.EventRaised += SaveFinished;
        }

        private void OnDisable()
        {
            _saveFinishedEventChannel.EventRaised -= SaveFinished;
        }

        private void OnApplicationFocus(bool focus)
        {
            OnFocusChanged(focus ? 1 : 0);
        }

        private void OnApplicationQuit()
        {
            OnBeforeUnloaded();
        }

        private void OnBeforeUnloaded()
        {
            UploadGameSave();
        }

        private void OnFocusChanged(int hasFocus)
        {
            if (hasFocus == 0)
            {
                UploadGameSave();
            }
        }

        private DateTime _lastUploadTime = DateTime.Now;

        private void UploadGameSave()
        {
            Debug.Log("ApplicationEventListener::UploadGameSave() - Uploading profile...");

            var timeToCheck = _lastUploadTime.AddSeconds(_saveInterval);
            if (DateTime.Compare(timeToCheck, DateTime.Now) > 0)
            {
                Debug.Log("ApplicationEventListener::UploadGameSave() - Uploading aborted!");
                return;
            }

            _forceSaveEvent.RaiseEvent();
        }

        private void SaveFinished()
        {
            _lastUploadTime = DateTime.Now;
        }
    }
}