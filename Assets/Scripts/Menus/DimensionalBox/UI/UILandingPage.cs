using System;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.UI
{
    public class UILandingPage : MonoBehaviour
    {
        [field: SerializeField] public GameObject DefaultSelectedButton { get; private set; }
        public event Action TransferringEquipments;
        public event Action TransferringMetaD;

        public void OnTransferEquipments() => TransferringEquipments?.Invoke();
        public void OnTransferMetaD() => TransferringMetaD?.Invoke();
    }
}