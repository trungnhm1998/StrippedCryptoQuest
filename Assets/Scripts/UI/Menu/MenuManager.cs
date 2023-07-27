using System;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.UI.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [Serializable]
        public struct MenuMap
        {
            public MenuTypeSO TypeSO;
            public UIMenuPanel Panel;
        }

        [SerializeField] private List<MenuMap> _panels = new();
        private Dictionary<EMenuType, UIMenuPanel> _cachedPanel = new();
        private UIMenuPanel _currentActivePanel;
        private void Awake()
        {
            _cachedPanel = new();
            foreach (var menuMap in _panels)
            {
                _cachedPanel.Add(menuMap.TypeSO.Type, menuMap.Panel);
            }

            _currentActivePanel = _cachedPanel[EMenuType.Main];
        }

        public void MenuHeaderButtonPressed(MenuTypeSO typeSO)
        {
            Debug.Log(typeSO.Type);
            if (_currentActivePanel.TypeSO.Type == typeSO.Type)
            {
                return;
            }
        }
    }
}