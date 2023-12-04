using UnityEngine;

namespace CryptoQuest.UI.Utilities
{
    public static class ComponentUtils
    {
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            return component.TryGetComponent<T>(out var foundComponent)
                ? foundComponent
                : component.gameObject.AddComponent<T>();
        }
    }
}