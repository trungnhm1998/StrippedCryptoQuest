using System.Collections;
using CryptoQuest.Core;
using CryptoQuest.Item.Equipment;
using TinyMessenger;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.UI.Tooltips
{
    public class ShowEquipmentTooltip : ActionBase
    {
        public EquipmentInfo Equipment { get; set; }
    }

    public class HideEquipmentTooltip : ActionBase { }

    public class EquipmentTooltipController : MonoBehaviour
    {
        [SerializeField] private float _autoReleaseTime = 5f;
        [SerializeField] private AssetReferenceT<GameObject> _equipmentTooltipAsset;
        private UIEquipmentTooltip _tooltip;
        private AsyncOperationHandle<GameObject> _handle;
        private TinyMessageSubscriptionToken _showTooltip;
        private TinyMessageSubscriptionToken _hideTooltip;
        private Coroutine _releaseCo;

        private void OnEnable()
        {
            _showTooltip = ActionDispatcher.Bind<ShowEquipmentTooltip>(ShowTooltip);
            _hideTooltip = ActionDispatcher.Bind<HideEquipmentTooltip>(HideTooltip);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_showTooltip);
            ActionDispatcher.Unbind(_hideTooltip);
        }

        private void ShowTooltip(ShowEquipmentTooltip ctx)
        {
            if (_releaseCo != null)
            {
                StopCoroutine(_releaseCo);
                _releaseCo = null;
            }

            StartCoroutine(LoadAndShowTooltipCo(ctx.Equipment));
        }

        private void HideTooltip(HideEquipmentTooltip _)
        {
            _tooltip.gameObject.SetActive(false);
            _releaseCo ??= StartCoroutine(CoAutoRelease());
        }

        private IEnumerator CoAutoRelease()
        {
            yield return new WaitForSeconds(_autoReleaseTime);
            if (_handle.IsValid()) Addressables.Release(_handle);
            _handle = new AsyncOperationHandle<GameObject>();
            _tooltip = null;
            _releaseCo = null;
        }

        private IEnumerator LoadAndShowTooltipCo(EquipmentInfo equipment)
        {
            yield return LoadTooltipCo();
            _tooltip.gameObject.SetActive(true);
            _tooltip.Init(equipment);
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