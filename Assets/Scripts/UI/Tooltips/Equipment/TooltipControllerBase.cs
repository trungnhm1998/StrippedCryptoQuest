using System.Collections;
using CryptoQuest.UI.Tooltips.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.UI.Tooltips.Equipment
{
    public class TooltipControllerBase<TTooltip> : MonoBehaviour where TTooltip : UITooltipBase
    {
        [SerializeField] private ShowTooltipEvent _showTooltipEvent;
        [SerializeField] private float _autoReleaseTime = 5f;
        [SerializeField] private AssetReferenceT<GameObject> _tooltipPrefabAsset;

        private TTooltip _tooltip;
        private AsyncOperationHandle<GameObject> _handle;
        private TinyMessageSubscriptionToken _showTooltip;
        private TinyMessageSubscriptionToken _hideTooltip;
        private Coroutine _releaseCo;

        private void OnEnable() => _showTooltipEvent.EventRaised += ShowTooltip;

        private void OnDisable() => _showTooltipEvent.EventRaised -= ShowTooltip;

        private void ShowTooltip(bool isShow)
        {
            if (!isShow)
            {
                HideTooltip();
                return;
            }

            StopReleasingTooltip();
            StartCoroutine(LoadAndShowTooltipCo());
        }

        private void HideTooltip()
        {
            if (_tooltip) _tooltip.gameObject.SetActive(false);
            _releaseCo ??= StartCoroutine(CoAutoRelease());
        }

        private void StopReleasingTooltip()
        {
            if (_releaseCo == null) return;
            StopCoroutine(_releaseCo);
            _releaseCo = null;
        }

        private IEnumerator LoadAndShowTooltipCo()
        {
            yield return LoadTooltipCo();
            _tooltip.gameObject.SetActive(true);
        }

        private IEnumerator CoAutoRelease()
        {
            yield return new WaitForSeconds(_autoReleaseTime);
            if (_handle.IsValid())
            {
                _tooltip.Hide -= HideTooltip;
                Addressables.Release(_handle);
            }
            _handle = new AsyncOperationHandle<GameObject>();
            _tooltip = null;
            _releaseCo = null;
        }

        private IEnumerator LoadTooltipCo()
        {
            if (_tooltip != null) yield break;
            if (_handle.IsValid() && _handle.Result != null) yield break;
            _handle = _tooltipPrefabAsset.InstantiateAsync(transform);
            yield return _handle;
            _handle.Result.SetActive(false);
            _tooltip = _handle.Result.GetComponent<TTooltip>();
            _tooltip.Hide += HideTooltip;
        }
    }
}