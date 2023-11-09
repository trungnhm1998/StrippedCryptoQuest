using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Menu;
using CryptoQuest.Tavern.Interfaces;
using CryptoQuest.UI.Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI.CharacterReplacement
{
    public class UICharacterList : MonoBehaviour
    {
        public static event UnityAction<UICharacterList> Rendered;

        [SerializeField] protected Transform _scrollRectContent;
        [SerializeField] protected UITavernItem _itemPrefab;
        [SerializeField] protected RectTransform _tooltipSafeArea;

        private ITooltip _tooltip;

        private List<ICharacterData> _characterList = new List<ICharacterData>();
        public List<ICharacterData> Data => _characterList;

        private List<UITavernItem> _cachedItems = new();

        private void Awake()
        {
            _tooltip = TooltipFactory.Instance.GetTooltip(ETooltipType.Equipment);
        }

        public void SetData(List<ICharacterData> data)
        {
            _characterList = data;
            CleanUpScrollView();
            RenderData();
            _tooltip.SetSafeArea(_tooltipSafeArea);
        }

        public void SelectDefault()
        {
            StartCoroutine(CoSetDefaultSelection());
        }

        private IEnumerator CoSetDefaultSelection()
        {
            yield return null;
            var firstButton = _scrollRectContent.GetComponentInChildren<MultiInputButton>();
            firstButton.Select();
        }

        private void CleanUpScrollView()
        {
            foreach (Transform child in _scrollRectContent)
            {
                Destroy(child.gameObject);
            }
        }

        private void RenderData()
        {
            _cachedItems.Clear();
            foreach (var itemData in _characterList)
            {
                var item = Instantiate(_itemPrefab, _scrollRectContent);
                item.SetItemInfo(itemData);
                IdentifyItemParentThenCacheItem(item);
            }
            Rendered?.Invoke(this);
        }

        private void IdentifyItemParentThenCacheItem(UITavernItem item)
        {
            item.Parent = _scrollRectContent;
            _cachedItems.Add(item);
        }

        public void SetInteractableAllButtons(bool isEnabled)
        {
            foreach (Transform item in _scrollRectContent)
            {
                item.GetComponent<Button>().enabled = isEnabled;
            }
        }

        public void UpdateList()
        {
            foreach (var item in _cachedItems)
            {
                item.EnablePendingTag(false);
            }
        }

        private void OnDisable()
        {
            CleanUpScrollView();
        }
    }
}