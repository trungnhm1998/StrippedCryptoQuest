using System;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Beast
{
    public class BeastTypeSO : ScriptableObject
    {
        [Serializable]
        public class Information
        {
            public int Id;
            public LocalizedString LocalizedName;
            public LocalizedString LocalizedDescription;
        }

        [field: SerializeField] public Information BeastInformation { get; private set; }
        public int Id => BeastInformation.Id;
    }
}