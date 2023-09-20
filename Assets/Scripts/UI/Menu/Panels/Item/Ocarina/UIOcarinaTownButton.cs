using System;
using CryptoQuest.Item.Ocarina;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Menu.Panels.Item.Ocarina
{
    public class UIOcarinaTownButton : MonoBehaviour
    {
        public event Action<OcarinaEntrance> Clicked;
        [SerializeField] private LocalizeStringEvent _townName;

        private OcarinaEntrance _location;

        public void SetTownName(OcarinaEntrance location)
        {
            _location = location;
            _townName.StringReference = location.MapName;
        }

        public void OnClicked()
        {
            Debug.Log($"OcarinaTownButton Clicked {_location}");
            Clicked?.Invoke(_location);
        }
    }
}