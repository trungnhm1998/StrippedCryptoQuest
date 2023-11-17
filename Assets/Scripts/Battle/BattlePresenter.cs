using System;
using System.Collections.Generic;
using CryptoQuest.Battle.UI.SelectCommand;
using CryptoQuest.Battle.UI.StartBattle;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public class BattlePresenter : MonoBehaviour
    {
        [field: SerializeField] public GameObject CommandPanel { get; private set; }
        [field: SerializeField] public GameObject BattleUI { get; private set; }
        [field: SerializeField] public UIIntroBattle IntroUI { get; private set; }
        [field: SerializeField] public UISelectCommand CommandUI { get; private set; }

        private readonly Dictionary<Type, Component> _cachedComponents = new();
        
        /// <summary>
        /// Same as Unity's <see cref="GameObject.TryGetComponent{T}(out T)"/> but with a cache
        /// </summary>
        public new bool TryGetComponent<T>(out T component) where T : Component
        {
            var type = typeof(T);
            if (!_cachedComponents.TryGetValue(type, out var value))
            {
                if (base.TryGetComponent(out component))
                    _cachedComponents.Add(type, component);

                return component != null;
            }

            component = (T)value;
            return true;
        }

        private void OnDisable()
        {
            _cachedComponents.Clear();
        }
    }
}