using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class EvolvePresenter : MonoBehaviour
    {
        [SerializeField] private UnityEvent<List<IEvolvableData>> _getInventoryEvent;

        private IEvolvableEquipment _equipmentModel;
        private List<IEvolvableData> _gameData = new();

        private void Start()
        {
            StartCoroutine(GetEquipments());
        }

        private IEnumerator GetEquipments()
        {
            _equipmentModel = GetComponentInChildren<IEvolvableEquipment>();
            yield return _equipmentModel.CoGetData();

            if (_equipmentModel.EvolvableData.Count <= 0) yield break;

            _gameData = _equipmentModel.EvolvableData;
            _getInventoryEvent.Invoke(_gameData);
        }

    }
}