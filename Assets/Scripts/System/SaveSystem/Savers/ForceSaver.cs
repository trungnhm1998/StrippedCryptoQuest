using System;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Savers
{
    [Serializable]
    public class ForceSaver : SaverBase
    {
        [SerializeField] private VoidEventChannelSO _forceSaveEvent;

        public override void RegistEvents()
        {
            _forceSaveEvent.EventRaised += Save;
        }

        public override void UnregistEvents()
        {
            _forceSaveEvent.EventRaised += Save;
        }

        private void Save()
        {
            _saveHandler.Save();
        }
    }
}