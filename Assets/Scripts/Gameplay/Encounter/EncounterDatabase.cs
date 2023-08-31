using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Gameplay.Encounter
{
    [CreateAssetMenu(fileName = "BattleFieldsDatabase", menuName = "Data/BattleFieldsDatabase")]
    public class EncounterDatabase : ScriptableObject
    {
        [Serializable]
        public struct Encounter
        {
            public string ID;
            public AssetReferenceT<EncounterData> Battlefield;
        }

        [SerializeField] private List<Encounter> _battlefields;

        private Dictionary<string, AssetReferenceT<EncounterData>> _lookup = new();
        private Dictionary<string, EncounterData> _loadedEncounters = new();

        private void OnEnable()
        {
            _lookup = new();
            _loadedEncounters = new();

            foreach (var map in _battlefields)
            {
                _lookup.Add(map.ID, map.Battlefield);
            }
        }

        private void OnDisable()
        {
            _lookup.Clear();
            _loadedEncounters.Clear();
        }

        public void PreloadEncounter(string encounterId)
        {
            if (!_lookup.TryGetValue(encounterId, out var encounter))
            {
                Debug.LogWarning($"Try to load encounter with id {encounterId} but not found in database {name}");
                return;
            }

            if (!encounter.RuntimeKeyIsValid())
            {
                Debug.LogWarning($"Encounter with id {encounterId} is not valid");
                return;
            }

            if (_loadedEncounters.ContainsKey(encounterId))
            {
                Debug.Log($"Encounter {encounterId} already loaded");
                return;
            }

            Addressables.LoadAssetAsync<EncounterData>(encounter).Completed += EncounterAreaDataLoaded;
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

        public EncounterData GetEncounterData(string encounterId)
        {
            if (!_loadedEncounters.TryGetValue(encounterId, out var encounterData))
            {
                Debug.LogWarning($"Encounter data with id {encounterId} not found in database {name}");
                encounterData = CreateInstance<EncounterData>(); // to avoid null reference exception
            }

            return encounterData;
        }
    }
}