using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem
{
    [Serializable]
    public class TeleportLocationsSaveSO : ScriptableObject
    {
        public List<string> VisitedLocations = new();
        public event Action Changed;


        public void AddVisitedLocation(string guid)
        {
            if (VisitedLocations.Contains(guid)) return;
            VisitedLocations.Add(guid);
            Changed?.Invoke();
        }
    }
}