using System.Collections;
using CryptoQuest.Menu;
using CryptoQuest.UI.Menu;
using UnityEngine;

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

        private IEnumerator CoSetDefaultSelection(Transform targetScrollRect = null)
        {
            var board = targetScrollRect ? targetScrollRect : _scrollRectContent;
            yield return new WaitForSeconds(.1f);

            var firstButton = board.GetComponentInChildren<MultiInputButton>();
            firstButton.Select();
        }

        protected virtual void CleanUpScrollView()
        {
            foreach (Transform child in _scrollRectContent)
            {
                Destroy(child.gameObject);
            }
        }

        protected virtual void SetParentIdentity(UITavernItem item)
        {
            item.Parent = _scrollRectContent;
        }

        protected abstract void RenderData();
    }
}