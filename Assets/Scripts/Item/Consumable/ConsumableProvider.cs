using CryptoQuest.Item;
using CryptoQuest.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest
{
    public interface IConsumableProvider
    {
        IEnumerator Load(ConsumableInfo consumable);
    }
    public class ConsumableProvider : MonoBehaviour, IConsumableProvider
    {
        [SerializeField] private ConsumableDatabase _consumableDatabase;

        private void Awake()
        {
            ServiceProvider.Provide<IConsumableProvider>(this);
        }

        public IEnumerator Load(ConsumableInfo consumable)
        {
            yield return _consumableDatabase.LoadDataById(consumable.DataId);
            var consumableSO = _consumableDatabase.GetDataById(consumable.DataId);

            consumable.Data = consumableSO;
        }
    }
}
