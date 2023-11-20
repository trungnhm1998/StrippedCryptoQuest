using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Ranch.UI
{
    public class UIBeastList : MonoBehaviour
    {
        private const float DEFAULT_TIME_TO_SELECT = 1f;

        [SerializeField] private Transform _scrollContent;
        [SerializeField] private UIBeastItem _beastItemPrefab;
        [SerializeField] private RectTransform _tooltipSafeArea;

        private List<Beast> _beastList = new();
        public List<Beast> Data => _beastList;
        private List<UIBeastItem> _cachedItems = new();

        private void OnDisable()
        {
            CleanUpScrollView();
        }

        public void SetData(List<Beast> data)
        {
            _beastList = data;
            CleanUpScrollView();
            RenderData();
        }

        public void SelectDefault() => Invoke(nameof(DefaultSelection), DEFAULT_TIME_TO_SELECT);

        public void SetEnableButtons(bool isEnable = true)
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
            Button firstButton = _scrollContent.GetComponentInChildren<Button>();
            if (firstButton) firstButton.Select();
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
            foreach (var itemData in _beastList)
            {
                var item = Instantiate(_beastItemPrefab, _scrollContent);
                item.SetItemInfo(itemData);
                IdentifyItemParentThenCacheItem(item);
            }
        }

        private void IdentifyItemParentThenCacheItem(UIBeastItem item)
        {
            item.Parent = _scrollContent;
            _cachedItems.Add(item);
        }
    }
}