using System.Collections.Generic;
using CryptoQuest.UI.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Ranch.UI
{
    public class UIBeastList : MonoBehaviour
    {
        [SerializeField] private Transform _scrollContent;
        [SerializeField] private UIBeastItem _beastItemPrefab;
        [SerializeField] private RectTransform _tooltipSafeArea;

        private ITooltip _tooltip;

        private List<UIBeastItem> _cachedItems = new();

        private void Awake()
        {
            // TODO: Wait for tooltip implementation
            // _tooltip = TooltipFactory.Instance.GetTooltip(ETooltipType.Beat);
        }

        private void OnDisable()
        {
            CleanUpScrollView();
        }

        public void SetData()
        {
            CleanUpScrollView();
            RenderData();
            // TODO: Wait for tooltip implementation
            // _tooltip.SetSafeArea(_tooltipSafeArea);
        }

        public void SelectDefault()
        {
            Invoke(nameof(DefaultSelection), 0.1f);
        }

        public void SetEnableButtons(bool isEnable)
        {
            foreach (Transform item in _scrollContent)
            {
                item.GetComponent<Button>().enabled = isEnable;
            }
        }

        public void UpdateList()
        {
            foreach (var item in _cachedItems)
            {
                item.EnablePendingTag(false);
            }
        }

        private void DefaultSelection()
        {
            var firstButton = _scrollContent.GetComponentInChildren<Button>();
            firstButton.Select();
        }

        private void CleanUpScrollView()
        {
            foreach (Transform child in _scrollContent)
            {
                Destroy(child.gameObject);
            }
        }

        private void RenderData()
        {
            _cachedItems.Clear();
            // TODO: Render data here
        }

        private void IdentifyItemParentThenCacheItem(UIBeastItem item)
        {
            item.Parent = _scrollContent;
            _cachedItems.Add(item);
        }
    }
}