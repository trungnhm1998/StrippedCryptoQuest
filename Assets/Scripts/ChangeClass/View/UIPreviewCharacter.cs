using AssetReferenceSprite;
using CryptoQuest.ChangeClass.API;
using CryptoQuest.UI.Character;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.ChangeClass.View
{
    public class UIPreviewCharacter : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private Image _element;
        [SerializeField] private Image _avatar;
        [SerializeField] private UIAttributeBar _expBar;
        [SerializeField] private TextMeshProUGUI _hp;
        [SerializeField] private TextMeshProUGUI _maxHp;
        [SerializeField] private TextMeshProUGUI _mp;
        [SerializeField] private TextMeshProUGUI _maxMp;
        [SerializeField] private TextMeshProUGUI _exp;
        [SerializeField] private TextMeshProUGUI _maxExp;
        [SerializeField] private TextMeshProUGUI _str;
        [SerializeField] private TextMeshProUGUI _vit;
        [SerializeField] private TextMeshProUGUI _agi;
        [SerializeField] private TextMeshProUGUI _int;
        [SerializeField] private TextMeshProUGUI _luck;
        [SerializeField] private TextMeshProUGUI _atk;
        [SerializeField] private TextMeshProUGUI _mAtk;
        [SerializeField] private TextMeshProUGUI _def;
        [SerializeField] private Sprite _randomElement;

        public void PreviewCharacter(PreviewCharacter data, UICharacter character, AssetReferenceT<Sprite> avatar, bool isSameElement)
        {
            _name.StringReference = character.Class.Origin.DetailInformation.LocalizedName;
            _element.sprite = isSameElement ? character.Class.Elemental.Icon : _randomElement;
            _hp.text = data.minHP.ToString();
            _maxHp.text = data.minHP.ToString();
            _mp.text = data.minMP.ToString();
            _maxMp.text = data.minMP.ToString();
            _str.text = data.minStrength.ToString();
            _vit.text = data.minVitality.ToString();
            _agi.text = data.minAgility.ToString();
            _int.text = data.minIntelligence.ToString();
            _luck.text = data.minLuck.ToString();
            _atk.text = data.minAttack.ToString();
            _mAtk.text = data.minMATK.ToString();
            _def.text = data.minDefence.ToString();
            LoadAssetReference(avatar);
        }

        public void PreviewNewCharacter(NewCharacter data, LocalizedString localized, Sprite element, AssetReferenceT<Sprite> avatar)
        {
            _name.StringReference = localized;
            _element.sprite = element;
            _hp.text = data.HP.ToString();
            _maxHp.text = data.HP.ToString();
            _mp.text = data.MP.ToString();
            _maxMp.text = data.MP.ToString();
            _exp.text = data.exp.ToString();
            _str.text = data.strength.ToString();
            _vit.text = data.vitality.ToString();
            _agi.text = data.agility.ToString();
            _int.text = data.intelligence.ToString();
            _luck.text = data.luck.ToString();
            _atk.text = data.attack.ToString();
            _def.text = data.deffence.ToString();
            _mAtk.text = data.MATK.ToString();
            LoadAssetReference(avatar);
        }

        public void UpdateExpBar(float currentExp, float requireExp)
        {
            _expBar.SetValue(currentExp);
            _expBar.SetMaxValue(requireExp);
        }

        private void LoadAssetReference(AssetReferenceT<Sprite> avatar)
        {
            if (avatar == null)
            {
                _avatar.enabled = false;
                return;
            }
            StartCoroutine(avatar.LoadSpriteAndSet(_avatar));
        }
    }
}
