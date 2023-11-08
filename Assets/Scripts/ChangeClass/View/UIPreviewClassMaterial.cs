using CryptoQuest.ChangeClass.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.ChangeClass.View
{
    public class UIPreviewClassMaterial : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Image _element;
        [SerializeField] private Image _avatar;
        [SerializeField] private TextMeshProUGUI _minHP;
        [SerializeField] private TextMeshProUGUI _maxHP;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _minMP;
        [SerializeField] private TextMeshProUGUI _maxMP;
        [SerializeField] private TextMeshProUGUI _exp;
        [SerializeField] private TextMeshProUGUI _str;
        [SerializeField] private TextMeshProUGUI _vit;
        [SerializeField] private TextMeshProUGUI _agi;
        [SerializeField] private TextMeshProUGUI _int;
        [SerializeField] private TextMeshProUGUI _luck;
        [SerializeField] private TextMeshProUGUI _atk;
        [SerializeField] private TextMeshProUGUI _mAtk;
        [SerializeField] private TextMeshProUGUI _def;

        public void PreviewCharacter(UICharacter character)
        {
            _name.text = character.Class.name;
            _level.text = $"Lv{character.Class.level}";
            _minHP.text = character.Class.HP.ToString();
            _maxHP.text = character.Class.maxHp.ToString();
            _minMP.text = character.Class.MP.ToString();
            _maxMP.text = character.Class.maxMp.ToString();
            _str.text = character.Class.strength.ToString();
            _vit.text = character.Class.vitality.ToString();
            _exp.text = character.Class.exp.ToString();
            _agi.text = character.Class.minAgility.ToString();
            _int.text = character.Class.intelligence.ToString();
            _luck.text = character.Class.luck.ToString();
            _atk.text = character.Class.attack.ToString();
            _mAtk.text = character.Class.MATK.ToString();
            _def.text = character.Class.deffence.ToString();
        }
    }
}
