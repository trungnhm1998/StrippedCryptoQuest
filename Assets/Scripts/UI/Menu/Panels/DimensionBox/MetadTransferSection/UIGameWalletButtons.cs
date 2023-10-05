using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.MetadTransferSection
{
    public class UIGameWalletButtons : UIWalletButtons
    {
        public override void Send(float value)
        {
            _metadAPI.TransferDiamondToMetad(value);
        }
    }
}
