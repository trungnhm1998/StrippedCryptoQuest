using System.Collections;
using CryptoQuest.System;
using IndiGames.Core.Common;
using UnityEngine;

namespace CryptoQuest.Item.Consumable
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
            yield break; // TODO: REFACTOR
            // yield return _consumableDatabase.LoadDataById(consumable.DataId);
            // var consumableSO = _consumableDatabase.GetDataById(consumable.DataId);
            //
            // consumable.Data = consumableSO;
        }
    }
}
