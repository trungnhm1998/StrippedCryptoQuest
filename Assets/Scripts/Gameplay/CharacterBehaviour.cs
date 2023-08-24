using CryptoQuest.Gameplay.Character;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public interface ICharacter
    {
    }

    public class CharacterBehaviour : MonoBehaviour, ICharacter
    {
        [SerializeField] private bool _initOnStart = true; // Maybe remove this later
        [field: SerializeField] public AbilitySystemBehaviour GameplayAbilitySystem { get; set; }
        [SerializeField] private CharacterSO _characterSO;
        public Elemental Element => _characterSO.Element;

        private IStatInitializer _statsInitializer;

        private void OnValidate()
        {
            if (GameplayAbilitySystem == null) GameplayAbilitySystem = GetComponent<AbilitySystemBehaviour>();
        }

        private void Awake()
        {
            _statsInitializer = GetComponent<IStatInitializer>();
        }

        private void Start()
        {
            if (_initOnStart) Init();
        }


        private void Init()
        {
            // _statsInitializer.InitStats();
            // var charElement = _characterMetaData.Element;
            //
            // GameplayAbilitySystem.AttributeSystem.AddAttribute(charElement.AttackAttribute);
            // GameplayAbilitySystem.AttributeSystem.AddAttribute(charElement.ResistanceAttribute);
            // for (int i = 0; i < charElement.Multipliers.Length; i++)
            // {
            //     var elementMultiplier = charElement.Multipliers[i];
            //     GameplayAbilitySystem.AttributeSystem.AddAttribute(elementMultiplier.Attribute);
            //     GameplayAbilitySystem.AttributeSystem.SetAttributeBaseValue(elementMultiplier.Attribute,
            //         elementMultiplier.Value);
            // }
            //
            // GameplayAbilitySystem.AttributeSystem.UpdateAttributeValues();
        }
    }
}