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

        protected void AfterSaveData(bool isEquipmentListEmpty = false)
        {
            CleanUpScrollView();
            RenderData();
            _tooltip.SetSafeArea(_tooltipSafeArea);
            CheckEmptyGameEquipmentList(isEquipmentListEmpty);
        }

        /// <summary>
        /// If the equipment list of Game board is not empty then the Wallet board will not run the SetDefaultSelection() method.
        /// </summary>
        /// <param name="isGameEquipmentListEmpty"></param>
        private void CheckEmptyGameEquipmentList(bool isGameEquipmentListEmpty = false)
        {
            if (!isGameEquipmentListEmpty) return;
            SetDefaultSelection();
        }

        public void SetDefaultSelection(Transform targetScrollRect = null)
        {
            StartCoroutine(CoSetDefaultSelection(targetScrollRect));
        }

        // Must delay this method a bit to prevent bug of unity event system
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