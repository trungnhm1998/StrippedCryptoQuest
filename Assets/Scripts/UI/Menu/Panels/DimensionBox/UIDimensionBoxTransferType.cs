using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox
{
    public class UIDimensionBoxTransferType : MonoBehaviour
    {
        [SerializeField] private UIDimensionBoxTabButton _defaultSelection;
        public void Init()
        {
            _defaultSelection.Select();
        }
    }
}
