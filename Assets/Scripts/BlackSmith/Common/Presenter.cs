using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Common
{
    public class Presenter : MonoBehaviour
    {
        private readonly Dictionary<Type, Component> _cachedComponents = new();

        public new bool TryGetComponent<T>(out T component, bool includeChildren = false) where T : Component
        {
            Type type = typeof(T);
            if (!_cachedComponents.TryGetValue(type, out var value))
            {

                if (base.TryGetComponent(out component))
                    _cachedComponents.Add(type, component);

                if (includeChildren && component == null)
                {
                    component = GetComponentInChildren<T>(true);
                    if (component != null)
                        _cachedComponents.Add(type, component);
                }

                return component != null;
            }

            component = (T)value;
            return true;
        }
    }
}
