using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Map;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace CryptoQuest
{
    public class Ocarina : MonoBehaviour, IUsable
    {
        [SerializeField] private MapStorageSO _mapStorage;
        [SerializeField] private PathStorageSO _pathStorage;
        [SerializeField] private LoadSceneEventChannelSO _requestLoadMapEvent;
        [SerializeField] private SceneScriptableObject _worldMapScene;
        [SerializeField] private OcarinaDataSO _ocarinaDatas;
        [SerializeField] private OcarinaData _currentSelectedOcarinaData;

        public void SelectDestination(OcarinaData selectedDestination)
        {
            foreach (var ocarinaData in _ocarinaDatas.ocarinaDataList)
            {
                if (ocarinaData.mapName == selectedDestination.mapName)
                    _currentSelectedOcarinaData = ocarinaData;
            }
        }

        public void Use()
        {
            foreach (var blockScene in _ocarinaDatas.ocarinaBlockSceneList)
            {
                if (_mapStorage.currentMapScene == blockScene) return;
            }

            TriggerTeleportation(_worldMapScene, _currentSelectedOcarinaData.path);
        }

        public void TriggerTeleportation(SceneScriptableObject sceneSO, MapPathSO pathSO)
        {
            _pathStorage.LastTakenPath = pathSO;
            _requestLoadMapEvent.RequestLoad(sceneSO);
        }
    }

    [Serializable]
    public class OcarinaData
    {
        public LocalizedString mapName;
        public MapPathSO path;
    }
}