using CryptoQuest.Events;
using CryptoQuest.Item.Ocarinas.Data;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.EditorTool
{
    public class OcarinaEditorCheat : MonoBehaviour
    {
        [SerializeField] private OcarinaDefinition _ocarinaData;
        [SerializeField] private MapPathEventChannelSO _destinationSelectedEvent;
        [SerializeField] private VoidEventChannelSO _destinationConfirmEvent;
        [SerializeField] private float _guiWidth = 400;
        [SerializeField] private float _guiButtonHeight = 50;
        [SerializeField] private int _fontSize = 20;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private bool _showOcarinaList;

        private void OnGUI()
        {
            GUI.skin.label.fontSize = _fontSize;
            _showOcarinaList =
                GUILayout.Toggle(_showOcarinaList, "Show Ocarina destinations", GUILayout.Width(_guiWidth));

            if (!_showOcarinaList) return;

            if (_ocarinaData.Locations.Count <= 0) return;

            foreach (var location in _ocarinaData.Locations)
            {
                if (!GUILayout.Button($"Use Ocarina go to {location.MapName.GetLocalizedString()}",
                        GUILayout.Width(_guiWidth),
                        GUILayout.Height(_guiButtonHeight))) continue;
                
                _destinationSelectedEvent.RaiseEvent(location.Path);
                _destinationConfirmEvent.RaiseEvent();
            }
        }
#endif
    }
}