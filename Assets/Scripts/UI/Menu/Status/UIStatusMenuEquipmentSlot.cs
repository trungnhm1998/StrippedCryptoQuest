using System;
using System.Collections;
using System.Collections.Generic;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

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
