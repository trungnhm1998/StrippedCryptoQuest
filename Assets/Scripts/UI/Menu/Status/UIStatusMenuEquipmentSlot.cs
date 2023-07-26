using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Status
{
    public class UIStatusMenuEquipmentSlot : MonoBehaviour
    {
        [SerializeField] private GameObject _selectEffect;

        public void Select()
        {
            _selectEffect.SetActive(true);
        }

        public void Deselect()
        {
            _selectEffect.SetActive(false);
        }
    }
}
