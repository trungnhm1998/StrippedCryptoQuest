using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Ranch.UI
{
    public class UIBeastOrganization : UIAbstractFarm
    {
        [FormerlySerializedAs("_walletBeatList")] [SerializeField] private UIBeastList walletBeastList;
        [FormerlySerializedAs("_inGameBeatList")] [SerializeField] private UIBeastList inGameBeastList;

        [SerializeField] private Transform _walletBeatListContent;
        [SerializeField] private Transform _inGameBeatListContent;

        private List<int> _selectedWalletBeatIds = new();
        private List<int> _selectedInGameBeatIds = new();

        private void OnDestroy() => StopAllCoroutines();

        public void Transfer(UIBeastItem item)
        {
            _selectedWalletBeatIds.Clear();
            _selectedInGameBeatIds.Clear();

            Transform currentList = item.Parent;
            Transform otherList;

            if (currentList == _inGameBeatListContent)
            {
                otherList = _walletBeatListContent;
                _selectedInGameBeatIds.Add(item.Id);
            }
            else
            {
                otherList = _inGameBeatListContent;
                _selectedWalletBeatIds.Add(item.Id);
            }

            item.Transfer(otherList);

            walletBeastList.SetEnableButtons(otherList == _walletBeatListContent);
            inGameBeastList.SetEnableButtons(otherList == _inGameBeatListContent);
        }

        public void SwitchList(Vector2 direction)
        {
            if (_inGameBeatListContent.childCount <= 0) return;
            if (_walletBeatListContent.childCount <= 0) return;

            switch (direction.x)
            {
                case > 0:
                    walletBeastList.SetEnableButtons(false);
                    FocusList(inGameBeastList);
                    break;
                case < 0:
                    inGameBeastList.SetEnableButtons(false);
                    FocusList(walletBeastList);
                    break;
            }
        }

        public void HandleListEnable() => StartCoroutine(nameof(CoHandleListEnable));

        private void FocusList(UIBeastList list)
        {
            list.SetEnableButtons(true);
            list.SelectDefault();
        }


        private IEnumerator CoHandleListEnable()
        {
            yield return new WaitUntil(() =>
                (_walletBeatListContent != null && _inGameBeatListContent != null) &&
                (_walletBeatListContent.childCount > 0 || _inGameBeatListContent.childCount > 0));

            switch (_walletBeatListContent.childCount)
            {
                case > 0:
                    inGameBeastList.SetEnableButtons(false);
                    walletBeastList.SelectDefault();
                    break;
                case <= 0:
                    inGameBeastList.SelectDefault();
                    break;
            }
        }
    }
}