using System.Collections;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.UI.Menu.Panels.Status;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection
{
    public abstract class UIEquipmentList : MonoBehaviour
    {
        [SerializeField] protected ScrollRect _scrollRect;
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

        // This method subscribe to the HideDialogEvent and also is called when the _switchToWalletBoardEvent/_switchToGameBoardEvent invokes on scene.
        public void SetDefaultSelection()
        {
            StartCoroutine(CoSetDefaultSelection());
        }

        // Must delay this method a bit to let the scroll view finish initializing.
        private IEnumerator CoSetDefaultSelection()
        {
            yield return new WaitForSeconds(.1f);

            var firstButton = _scrollRect.content.GetComponentInChildren<MultiInputButton>();
            firstButton.Select();
        }

        protected virtual void CleanUpScrollView()
        {
            foreach (Transform child in _scrollRect.content)
            {
                Destroy(child.gameObject);
            }
        }

        protected virtual void SetParentIdentity(UITransferItem item)
        {
            item.Parent = _scrollRect.content;
        }

        protected abstract void RenderData();
    }
}
