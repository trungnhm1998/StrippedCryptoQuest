using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest;
using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Events;
using CryptoQuest.Map;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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
        private List<GoFrom> _cachedDestinations = new();

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
                        if (SceneManager.GetSceneByName("WorldMap").isLoaded)
                        {
                            if (_cachedDestinations.Count == 0)
                                _cachedDestinations = new List<GoFrom>(FindObjectsOfType<GoFrom>());
                            foreach (GoFrom destination in _cachedDestinations)
                            {
                                if (location.Path == destination.MapPath)
                                {
                                    GameObject hero = GameObject.FindObjectOfType<HeroBehaviour>().gameObject;
                                    hero.transform.position = destination.transform.position;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            _destinationSelectedEvent.RaiseEvent(location.Path);
                            _destinationConfirmEvent.RaiseEvent();
                        }
                    }
                }
            }
        }
#endif
    }
}