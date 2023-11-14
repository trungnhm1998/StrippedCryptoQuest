using System.Collections;
using CryptoQuest.Core;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Sagas;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.UI.Tooltips
{
    public class ShowEquipmentTooltip : ActionBase
    {
        public EquipmentInfo Equipment { get; }
        public bool IsShow { get; }

        public ShowEquipmentTooltip(EquipmentInfo equipment, bool isShow = true)
        {
            Equipment = equipment;
            IsShow = isShow;
        }
    }
    

    public class EquipmentTooltipController : SagaBase<ShowEquipmentTooltip>
    {
        [SerializeField] private AssetReferenceT<GameObject> _equipmentTooltipAsset;
        private UIEquipmentTooltip _tooltip;
        private AsyncOperationHandle<GameObject> _handle;

        protected override void HandleAction(ShowEquipmentTooltip ctx)
        {
            if (ctx.IsShow)
                StartCoroutine(LoadAndShowTooltipCo(ctx.Equipment));
            else
            {
                if (_tooltip == null)
                {
                    return;
                }
                _tooltip.gameObject.SetActive(false);
            }
        }

        private IEnumerator LoadAndShowTooltipCo(EquipmentInfo equipment)
        {
            yield return LoadTooltipCo();
            _tooltip.Init(equipment);
            _tooltip.gameObject.SetActive(true);
        }

        private IEnumerator LoadTooltipCo()
        {
            if (_handle.IsValid() && _handle.Result != null) yield break;
            _handle = _equipmentTooltipAsset.InstantiateAsync(transform);
            yield return _handle;
            _tooltip = _handle.Result.GetComponent<UIEquipmentTooltip>();
            _tooltip.gameObject.SetActive(false);
        }
    }
}