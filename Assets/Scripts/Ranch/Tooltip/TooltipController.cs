using System.Collections;
using CryptoQuest.Beast;
using CryptoQuest.UI.Tooltips.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Ranch.Tooltip
{
    public class TooltipController : MonoBehaviour
    {
        [SerializeField] private UIBeastTooltip _uiTooltip;
        [SerializeField] private ShowTooltipEvent _showBeastTooltipChannelSO;
        [SerializeField] private float _delayTime = 0.1f;
        private IBeast _beast;

        private void OnEnable() => _showBeastTooltipChannelSO.EventRaised += ShowTooltip;

        private void OnDisable() => _showBeastTooltipChannelSO.EventRaised -= ShowTooltip;

        private void ShowTooltip(bool isShowDetail)
        {
            if (!isShowDetail)
            {
                HideTooltip();
                return;
            }
            CanShowTooltip();
        }

        private void HideTooltip()
        {
            _uiTooltip.gameObject.SetActive(false);
        }

        private void CanShowTooltip()
        {
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (selectedGameObject == null) return;
            var provider = selectedGameObject.GetComponent<ITooltipBeastProvider>();
            if (provider == null) return;
            if (provider.Beast == null || provider.Beast.IsValid() == false) return;
            _beast = provider.Beast;
            StartCoroutine(ShowTooltip(_delayTime));
        }

        private IEnumerator ShowTooltip(float time)
        {
            yield return new WaitForSeconds(time);
            _uiTooltip.gameObject.SetActive(true);
            _uiTooltip.Init(_beast);
        }
    }
}