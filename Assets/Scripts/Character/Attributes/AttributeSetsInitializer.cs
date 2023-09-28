using CryptoQuest.Character.Tag;
using UnityEngine;

namespace CryptoQuest.Character.Attributes
{
    public class AttributeSetsInitializer : MonoBehaviour
    {
        [SerializeField] private AttributeSets _attributeSets; // Just for the asset to load
        [SerializeField] private TagsDef _tags;
    }
}