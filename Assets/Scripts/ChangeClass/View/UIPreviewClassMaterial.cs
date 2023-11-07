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
            _name.text = character.Class.Name;
            _level.text = $"Lv{character.Class.Level}";
            _minHP.text = character.Class.Hp.ToString();
            _maxHP.text = character.Class.MaxHp.ToString();
            _minMP.text = character.Class.Mp.ToString();
            _maxMP.text = character.Class.MaxMp.ToString();
            _str.text = character.Class.Str.ToString();
            _vit.text = character.Class.Vit.ToString();
            _exp.text = character.Class.Exp.ToString();
            _agi.text = character.Class.Agi.ToString();
            _int.text = character.Class.Int.ToString();
            _luck.text = character.Class.Luck.ToString();
            _atk.text = character.Class.Atk.ToString();
            _mAtk.text = character.Class.MAtk.ToString();
            _def.text = character.Class.Def.ToString();
        }
    }
}
