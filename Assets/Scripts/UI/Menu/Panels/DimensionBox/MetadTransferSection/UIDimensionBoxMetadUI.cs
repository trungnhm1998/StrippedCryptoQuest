using System;
using CryptoQuest.Events;
using CryptoQuest.Events.UI.Dialogs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.MetadTransferSection
{
    public class UIDimensionBoxMetadUI : MonoBehaviour
    {
        [SerializeField] private Text _ingameWalletMetad;
        [SerializeField] private Text _web3WalletMetad;

        public void SetCurrentMetad(float ingameMetaD, float webMetaD)
        {
            _ingameWalletMetad.text = ingameMetaD.ToString();
            _web3WalletMetad.text = webMetaD.ToString();
        }
    }
}
