using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UIUpgradeResult : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _illustration;
        [SerializeField] private Image _rarity;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private List<Image> _listStar;

        public void RenderEquipment(IUpgradeEquipment equipment)
        {
            _displayName.StringReference = equipment.DisplayName;
            _icon.sprite = equipment.Icon;
            _rarity.sprite = equipment.Rarity;
            _level.text = equipment.Level.ToString();
            StartCoroutine(LoadSpriteAndSet(equipment.Illustration));
        }

        private IEnumerator LoadSpriteAndSet(AssetReferenceT<Sprite> illustration)
        {
            if (illustration.RuntimeKeyIsValid() == false) yield break;
            var handle = illustration.LoadAssetAsync<Sprite>();
            yield return handle;
            _illustration.sprite = handle.Result;
        }
    }
}