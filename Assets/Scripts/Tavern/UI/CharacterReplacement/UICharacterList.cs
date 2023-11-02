using System.Collections;
using CryptoQuest.Menu;
using CryptoQuest.UI.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI.CharacterReplacement
{
    public abstract class UICharacterList : MonoBehaviour
    {
        [SerializeField] protected Transform _scrollRectContent;
        [SerializeField] protected GameObject _singleItemPrefab;
        [SerializeField] protected RectTransform _tooltipSafeArea;

        private ITooltip _tooltip;

        private void Awake()
        {
            _tooltip = TooltipFactory.Instance.GetTooltip(ETooltipType.Equipment);
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

        protected abstract void RenderData();

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