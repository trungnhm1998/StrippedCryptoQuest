using System;
using CryptoQuest.Item.Ocarinas.Data;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Menu.Panels.Item.Ocarina
{
    public class OcarinaTownButton : MonoBehaviour
    {
        public event Action<OcarinaDefinition.Location> Clicked;
        [SerializeField] private LocalizeStringEvent _townName;

        private OcarinaDefinition.Location _location;
        public void SetTownName(OcarinaDefinition.Location localtion)
        {
            _location = localtion;
            _townName.StringReference = localtion.MapName;
        }

        public void OnClicked()
        {
            Debug.Log($"OcarinaTownButton Clicked {_location.Path}");
            Clicked?.Invoke(_location);
        }
    }
}