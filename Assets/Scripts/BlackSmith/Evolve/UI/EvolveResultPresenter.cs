using CryptoQuest.BlackSmith.Common;
using CryptoQuest.BlackSmith.Interface;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class EvolveResultPresenter : Presenter
    {
        [field: SerializeField] public UIResultPanel EvolveResultUI { get; private set; }

        private void OnEnable()
        {
            EvolveResultUI.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            EvolveResultUI.gameObject.SetActive(false);
        }

        public void SetResultFail(IEvolvableEquipment equipment)
        {
            EvolveResultUI.SetFailInfo(equipment);
        }

        public void SetResultSuccess(IEvolvableEquipment equipment)
        {
            EvolveResultUI.SetSuccessInfo(equipment);
        }
    }
}
