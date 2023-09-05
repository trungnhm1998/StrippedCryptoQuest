using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Gameplay.Encounter
{
    public class EncounterDatabase : ScriptableObject
    {
        [Serializable]
        public struct Encounter
        {
            public string ID;
            public AssetReferenceT<EncounterData> Battlefield;
        }

        [SerializeField] private List<Encounter> _battlefields;

        private Dictionary<string, AssetReferenceT<EncounterData>> _lookup = null;

        private Dictionary<string, AssetReferenceT<EncounterData>> Lookup
        {
            get
            {
                if (_lookup != null && _lookup.Count > 0)
                    return _lookup;

                _lookup = new();
                _loadedEncounters = new();

                foreach (var map in _battlefields)
                {
                    _lookup.Add(map.ID, map.Battlefield);
                }

                return _lookup;
            }
        }

        private Dictionary<string, EncounterData> _loadedEncounters = new();

        private void OnDisable()
        {
            Lookup.Clear();
            _loadedEncounters.Clear();
        }

        public AsyncOperationHandle<EncounterData> PreloadEncounter(string encounterId)
        {
            if (!Lookup.TryGetValue(encounterId, out var encounter))
            {
                Debug.LogWarning($"Try to load encounter with id \"{encounterId}\" but not found in database {name}");
                return default;
            }

            if (!encounter.RuntimeKeyIsValid())
            {
                Debug.LogWarning($"Encounter with id {encounterId} is not valid");
                return default;
            }

            if (_loadedEncounters.ContainsKey(encounterId))
            {
                Debug.Log($"Encounter {encounterId} already loaded");
                return default;
            }

            var handle = Addressables.LoadAssetAsync<EncounterData>(encounter);
            handle.Completed += EncounterAreaDataLoaded;
            return handle;
        }

        private void EncounterAreaDataLoaded(AsyncOperationHandle<EncounterData> encounterAsyncOp)
        {
            if (encounterAsyncOp.Status == AsyncOperationStatus.Succeeded)
            {
                EncounterData encounterData = encounterAsyncOp.Result;
                _loadedEncounters.Add(encounterData.ID, encounterData);
                Debug.Log($"Encounter {encounterData.name} loaded");
            }
            else
            {
                Debug.LogWarning($"Encounter data loaded failed with error {encounterAsyncOp.OperationException}");
            }
        }

        public bool TryGetEncounterData(string encounterId, out EncounterData encounter)
        {
            if (!_loadedEncounters.TryGetValue(encounterId, out var encounterData))
            {
                Debug.LogWarning($"Encounter data with id {encounterId} not yet loaded");
                encounter = null;
                return false;
            }

            encounter = encounterData;
            return true;
        }
    }
}