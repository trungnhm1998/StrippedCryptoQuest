using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UIEquipmentDetail : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private Image _illustration;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private GameObject[] _stars = new GameObject[5];
        [SerializeField] private Image _rarity;

        public void SetEquipmentDetail(IEvolvableData equipment)
        {
            BeforeSetData();

            _icon.sprite = equipment.Icon;
            _localizedName.StringReference = equipment.LocalizedName;
            _illustration.sprite = equipment.Icon;
            _level.text = $"Lv {equipment.Level}";

            for (int i = 0; i < equipment.Stars; i++)
            {
                _stars[i].SetActive(true);
            }

            _rarity.sprite = equipment.Rarity;
        }

        private void BeforeSetData()
        {
            for (int i = 0; i < _stars.Length; i++)
            {
                _stars[i].SetActive(false);
            }
        }
    }
}