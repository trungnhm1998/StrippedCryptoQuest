using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox
{
    public abstract class UITransferSection : MonoBehaviour
    {
        [SerializeField] private GameObject _contents;

        public void EnterTransferSection()
        {
            _contents.SetActive(true);
        }

        public void ExitTransferSection()
        {
            _contents.SetActive(false);
        }
    }
}
