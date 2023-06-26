using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Indigames.AbilitySystem
{
    [RequireComponent(typeof(AttributeSystem))]
    [RequireComponent(typeof(SkillSystem))]
    [RequireComponent(typeof(EffectSystem))]
    [RequireComponent(typeof(TagSystem))]
    public class AbilitySystem : MonoBehaviour
    {
        [SerializeField] private AttributeSystem _attributeSystem;
        public AttributeSystem AttributeSystem => _attributeSystem;

        [SerializeField] private SkillSystem _skillSystem;
        public SkillSystem SkillSystem => _skillSystem;

        [SerializeField] private EffectSystem _effectSystem;
        public EffectSystem EffectSystem => _effectSystem;

        [SerializeField] private TagSystem _tagSystem;
        public TagSystem TagSystem => _tagSystem;


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_attributeSystem != null) return;
            _attributeSystem = GetComponent<AttributeSystem>();
            if (_skillSystem != null) return;
            _skillSystem = GetComponent<SkillSystem>();
            if (_effectSystem != null) return;
            _effectSystem = GetComponent<EffectSystem>();
            if (_tagSystem != null) return;
            _tagSystem = GetComponent<TagSystem>();
        }
#endif

        private void Awake()
        {
            _skillSystem.InitSystem(this);
            _effectSystem.InitSystem(this);
        }
    }
}
