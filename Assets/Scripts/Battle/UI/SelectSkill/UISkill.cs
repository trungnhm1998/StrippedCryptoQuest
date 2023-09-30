using System;
using CryptoQuest.Character.Ability;
using TMPro;
using UnityEngine;

namespace CryptoQuest.Battle.UI.SelectSkill
{
    public class UISkill : MonoBehaviour
    {
        public Action<UISkill> Selected;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _cost;

        private CastableAbility _skill;
        public CastableAbility Skill => _skill;

        public void Init(CastableAbility skill)
        {
            _skill = skill;
            _name.text = skill.name;
        }

        public void OnPressed()
        {
            Selected?.Invoke(this);
        }
    }
}