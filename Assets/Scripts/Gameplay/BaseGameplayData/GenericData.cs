using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.BaseGameplayData
{
    [CreateAssetMenu(fileName = "GenericData", menuName = "Gameplay/BaseGameplayData/GenericData")]
    public class GenericData : ScriptableObject
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public LocalizedString Name { get; private set; }
        [field: SerializeField] public LocalizedString Description { get; private set; }
    }
}