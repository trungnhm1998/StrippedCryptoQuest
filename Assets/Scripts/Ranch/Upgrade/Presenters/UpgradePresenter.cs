using CryptoQuest.Beast;
using CryptoQuest.Ranch.Upgrade.UI;
using UnityEngine;

namespace CryptoQuest.Ranch.Upgrade.Presenters
{
    public class UpgradePresenter : MonoBehaviour
    {
        [SerializeField] private BeastInventorySO _inventorySo;
        [SerializeField] private UIBeastUpgradeList _beastList;
        [SerializeField] private UIBeastUpgradeDetail _uiBeastEvolveDetail;

        private void Start()
        {
            Initialization();
        }

        private void OnEnable()
        {
            _beastList.OnInspectingEvent += ShowBeastDetails;
        }

        private void ShowBeastDetails(UIBeastUpgradeListDetail ui)
        {
            _uiBeastEvolveDetail.SetupUI(ui.Beast);
        }

        private void Initialization()
        {
            var beasts = _inventorySo.OwnedBeasts;
            _beastList.FillBeasts(beasts);
        }
    }
}