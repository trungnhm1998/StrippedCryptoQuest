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
        private Coroutine _loadAndShowCo;

        private void OnEnable() => _showTooltipEvent.EventRaised += ShowTooltip;

        private void OnDisable() => _showTooltipEvent.EventRaised -= ShowTooltip;

        private void ShowTooltip(bool isShow)
        {
            if (!isShow)
            {
                HideTooltip();
                return;
            }

            InternalShowTooltipAsync();
        }


        private void Update()
        {
            if (_tooltip == null) return;
            if (_tooltip.gameObject.activeSelf) return;
            if (_releaseCo != null) return;
            _releaseCo = StartCoroutine(CoAutoRelease());
        }

        private void HideTooltip()
        {
            if (_tooltip) _tooltip.gameObject.SetActive(false);
            _releaseCo ??= StartCoroutine(CoAutoRelease());
        }

        private IEnumerator CoAutoRelease()
        {
            yield return new WaitForSeconds(_autoReleaseTime);
            if (_tooltip != null)
            {
                DestroyImmediate(_tooltip.gameObject);
                Addressables.Release(_handle);
            }

            _handle = default;
            _tooltip = null;
            _releaseCo = null;
        }

        private void InternalShowTooltipAsync()
        {
            if (_loadAndShowCo != null)
            {
                StopCoroutine(_loadAndShowCo);
                _loadAndShowCo = null;
            }

            _loadAndShowCo = StartCoroutine(LoadAndShowTooltipCo());
        }

        private IEnumerator LoadAndShowTooltipCo()
        {
            yield return LoadTooltipCo();
            StopReleasingTooltip();
            _tooltip.gameObject.SetActive(true);
            _loadAndShowCo = null;
        }

        private void StopReleasingTooltip()
        {
            if (_releaseCo == null) return;
            StopCoroutine(_releaseCo);
            _releaseCo = null;
        }

        private IEnumerator LoadTooltipCo()
        {
            if (_tooltip != null) yield break;
            if (!_handle.IsValid()) _handle = _tooltipPrefabAsset.LoadAssetAsync<GameObject>();
            yield return _handle;
            _handle.Result.SetActive(false);
            _tooltip = Instantiate(_handle.Result, transform).GetComponent<TTooltip>();
        }
    }
}