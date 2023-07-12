using CryptoQuest;
using CryptoQuest.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuestEditor.EditorTool
{
    public class OcarinaEditorCheat : MonoBehaviour
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        public OcarinaLocations OcarinaData;
        private bool _showOcarinaList;
        [SerializeField] private MapPathEventChannelSO _destinationSelectedEvent;
        [SerializeField] private VoidEventChannelSO _destinationConfirmEvent;
        [SerializeField] private float _guiWidth = 400;
        [SerializeField] private float _guiButtonHeight = 50;
        [SerializeField] private int _fontSize = 20;

        private void OnGUI()
        {
            GUI.skin.label.fontSize = _fontSize;
            _showOcarinaList =
                GUILayout.Toggle(_showOcarinaList, "Show Ocarina destinations", GUILayout.Width(_guiWidth));

            if (!_showOcarinaList) return;

            if (OcarinaData.Locations.Count > 0)
            {
                foreach (OcarinaLocations.Location location in OcarinaData.Locations)
                {
                    if (GUILayout.Button($"Use Ocarina go to {location.MapName.GetLocalizedString()}",
                            GUILayout.Width(_guiWidth),
                            GUILayout.Height(_guiButtonHeight)))
                    {
                        _destinationSelectedEvent.RaiseEvent(location.Path);
                        _destinationConfirmEvent.RaiseEvent();
                    }
                }
            }
        }
#endif
    }
}