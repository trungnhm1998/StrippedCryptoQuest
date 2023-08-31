using System;
using CryptoQuest.UI.Menu.Panels.Home;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Analytics;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Character
{
    /// <summary>
    /// Where this character come from?
    /// name
    /// age, height, weight
    /// </summary>
    public class CharacterBackgroundInfo : ScriptableObject
    {
        [Serializable]
        public struct Information
        {
            public int Id;
            public LocalizedString LocalizedName;
            public Gender Gender;
            public LocalizedString LocalizedBirthPlace;
            public float Height;
            public float Weight;
            public LocalizedString LocalizedDescription;
        }

        [field: SerializeField] public Information DetailInformation { get; set; }
        [field: SerializeField] public AssetLabelReference Label { get; private set; }

        public virtual void SetupUI(ICharacterInfo uiCharacterInfo)
        {
            uiCharacterInfo.SetLocalizedName(DetailInformation.LocalizedName);
        }
    }
}