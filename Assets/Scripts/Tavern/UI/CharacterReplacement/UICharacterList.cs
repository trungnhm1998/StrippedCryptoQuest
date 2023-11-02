using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Menu;
using CryptoQuest.Tavern.Interfaces;
using CryptoQuest.UI.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI.CharacterReplacement
{
    public class UICharacterList : MonoBehaviour
    {
        [SerializeField] protected Transform _scrollRectContent;
        [SerializeField] protected GameObject _singleItemPrefab;
        [SerializeField] protected RectTransform _tooltipSafeArea;

        private ITooltip _tooltip;

        private List<ICharacterData> _characterList = new List<ICharacterData>();

        private void Awake()
        {
            _tooltip = TooltipFactory.Instance.GetTooltip(ETooltipType.Equipment);
        }

        public void SetData(List<ICharacterData> data)
        {
            _characterList = data;
            StartCoroutine(AfterSaveData());
        }

        protected IEnumerator AfterSaveData()
        {
            CleanUpScrollView();
            RenderData();

            yield return null;

            yield return StartCoroutine(CoSetDefaultSelection());
            _tooltip.SetSafeArea(_tooltipSafeArea);
        }

        public IEnumerator CoSetDefaultSelection(Transform targetScrollRect = null)
        {
            var board = targetScrollRect ? targetScrollRect : _scrollRectContent;
            yield return new WaitForSeconds(.1f);

            var firstButton = board.GetComponentInChildren<MultiInputButton>();
            firstButton.Select();
        }

        private void CleanUpScrollView()
        {
            foreach (Transform child in _scrollRectContent)
            {
                Destroy(child.gameObject);
            }
        }

        protected void SetParentIdentity(UITavernItem item)
        {
            item.Parent = _scrollRectContent;
        }

        protected void RenderData()
        {
            foreach (var itemData in _characterList)
            {
                var item = Instantiate(_singleItemPrefab, _scrollRectContent).GetComponent<UITavernItem>();
                item.SetItemInfo(itemData);
                SetParentIdentity(item);
            }
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
            foreach (Transform item in _scrollRectContent)
            {
                item.GetComponent<UITavernItem>().EnablePendingTag(false);
            }
        }
    }
}