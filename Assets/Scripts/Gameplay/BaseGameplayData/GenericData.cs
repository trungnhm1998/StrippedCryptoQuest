using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.BaseGameplayData
{
    [CreateAssetMenu(fileName = "GenericData", menuName = "Gameplay/BaseGameplayData/GenericData")]
    public class GenericData : ScriptableObject
    {
        public int Id;
        public LocalizedString Name;
        public LocalizedString Description;
    }
}