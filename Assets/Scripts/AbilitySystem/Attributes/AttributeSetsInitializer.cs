using UnityEngine;

namespace CryptoQuest.AbilitySystem.Attributes
{
    public class AttributeSetsInitializer : MonoBehaviour
    {
        [SerializeField] private AttributeSets _attributeSets; // Just for the asset to load
        [SerializeField] private TagsDef _tags;
    }
}