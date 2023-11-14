using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;

namespace CryptoQuest.Sagas.Objects
{
    /// <summary>
    /// Provides config mapping between response and attribute, so we can map the response to the correct attribute
    /// such as response has attribute name "strength" and we have Strength attribute then we can map them together
    /// </summary>
    [Serializable]
    public struct ResponseAttributeMap
    {
        public string Name;
        public AttributeScriptableObject Attribute;
    }
}