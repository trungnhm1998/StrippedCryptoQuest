using System.Collections;
using CryptoQuest.Item.Consumable;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using UnityEngine;

namespace CryptoQuest.Sagas.Items
{
    public interface IConsumableResponseConverter
    {
        ConsumableInfo Convert(RewardResponse.Items response);
    }

    public class ConsumableResponseConverter : MonoBehaviour, IConsumableResponseConverter
    {
        [SerializeField] private ConsumableDatabase _database;
        private void Awake() => ServiceProvider.Provide<IConsumableResponseConverter>(this);

        public ConsumableInfo Convert(RewardResponse.Items response)
        {
            ConsumableInfo consumable = new ConsumableInfo()
            {
                Quantity = response.itemNum
            };
            StartCoroutine(CoLoadConsumable(consumable, response));
            return consumable;
        }

        private IEnumerator CoLoadConsumable(ConsumableInfo info, RewardResponse.Items response)
        {
            var consumableHandle = _database.LoadDataById(response.itemId);
            yield return consumableHandle;

            info.Data = _database.GetDataById(response.itemId);
        }
    }
}