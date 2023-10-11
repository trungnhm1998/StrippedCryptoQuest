using CryptoQuest.AbilitySystem.Attributes;
using UnityEditor;

namespace CryptoQuest.Tests.Runtime.Battle.Builder
{
    public class ElementBuilder
    {
        private const string ELEMENT_ASSET = "Assets/ScriptableObjects/Character/Attributes/Elemental/{0}/{0}.asset";

        private Elemental LoadElement(string element)
        {
            var path = string.Format(ELEMENT_ASSET, element);
            return AssetDatabase.LoadAssetAtPath<Elemental>(path);
        }

        public Elemental Fire => LoadElement("Fire");
        public Elemental Water => LoadElement("Water");
    }
}