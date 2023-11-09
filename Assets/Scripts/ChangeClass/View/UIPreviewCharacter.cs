using CryptoQuest.ChangeClass.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.ChangeClass.View
{
    public class UIPreviewCharacter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Image _element;
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

        public void PreviewCharacter(PreviewCharacter data, UICharacter character)
        {
            _name.text = character.Class.name;
            _hp.text = data.minHP.ToString();
            _mp.text = data.minMP.ToString();
            _str.text = data.minStrength.ToString();
            _vit.text = data.minVitality.ToString();
            _agi.text = data.minAgility.ToString();
            _int.text = data.minIntelligence.ToString();
            _luck.text = data.minLuck.ToString();
            _atk.text = data.minAttack.ToString();
            _mAtk.text = data.minMATK.ToString();
            _def.text = data.minDefence.ToString();
        }

        public void PreviewNewCharacter(NewCharacter data, UICharacter character)
        {
            _name.text = character.Class.name;
            _hp.text = data.HP.ToString();
            _maxHp.text = data.maxHP.ToString();
            _mp.text = data.MP.ToString();
            _maxMp.text = data.maxMP.ToString();
            _exp.text = data.exp.ToString();
            _str.text = data.strength.ToString();
            _vit.text = data.vitality.ToString();
            _agi.text = data.agility.ToString();
            _int.text = data.intelligence.ToString();
            _luck.text = data.luck.ToString();
            _atk.text = data.attack.ToString();
            _def.text = data.deffence.ToString();
            _mAtk.text = data.MATK.ToString();
        }
    }
}
