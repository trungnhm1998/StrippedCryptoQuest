using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Beast.UI
{
    public class UIBeastDetail : MonoBehaviour
    {
        [SerializeField] private TMP_Text _beastName;
        [SerializeField] private TMP_Text _beastLevel;
        [SerializeField] private TMP_Text _beastPassiveSkill;
        [SerializeField] private TMP_Text _beastStatus;
        [SerializeField] private Image _beastImage;

        private void OnEnable()
        {
            UIBeast.Inspecting += OnInspectingBeast;
        }

        private void OnDisable()
        {
            UIBeast.Inspecting -= OnInspectingBeast;
        }

        private void OnInspectingBeast(Sagas.Objects.Beast beast)
        {
            _beastName.text = beast.name;
            _beastLevel.text = beast.level.ToString();
        }
    }
}