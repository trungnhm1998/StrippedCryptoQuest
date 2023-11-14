using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Actions;
using CryptoQuest.Character;
using CryptoQuest.Character.Hero;
using CryptoQuest.Core;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using TinyMessenger;
using UniRx;
using UnityEngine;
using AttributeScriptableObject = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;

namespace CryptoQuest.Sagas.Profile
{
    public class FetchProfileCharacters : SagaBase<FetchProfileCharactersAction>
    {
        [SerializeField] private HeroInventorySO _heroInventory;
        [SerializeField] private List<Elemental> _elements = new();
        [SerializeField] private List<CharacterClass> _classes = new();

        [Tooltip("The order of character's name must match with the order of character's origin")]
        [SerializeField] private List<String> _charNames = new();

        [Tooltip("The order of character's name must match with the order of character's origin")]
        [SerializeField] private List<Origin> _charOrigins = new();

        [SerializeField] private List<ResponseAttributeMap> _attributeMap = new();

        private Dictionary<string, AttributeScriptableObject> _lookupAttribute = new();
        private FieldInfo[] _fields;

        private TinyMessageSubscriptionToken _heroInventoryUpdateEvent;

        private void Awake()
        {
            _lookupAttribute = _attributeMap.ToDictionary(map => map.Name, map => map.Attribute);
            _heroInventoryUpdateEvent = ActionDispatcher.Bind<GetGameNftCharactersSucceed>(RefreshHeroInventory);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ActionDispatcher.Unbind(_heroInventoryUpdateEvent);
        }

        private void RefreshHeroInventory(GetGameNftCharactersSucceed obj)
        {
            OnInventoryFilled(obj.InGameCharacters.ToArray());
        }

        protected override void HandleAction(FetchProfileCharactersAction _)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithParams(new Dictionary<string, string>()
                    { { "source", $"{((int)ECharacterStatus.InGame).ToString()}" } })
                .Get<CharactersResponse>(Networking.API.Profile.GET_CHARACTERS)
                .Subscribe(ProcessResponseCharacters, OnError);
        }

        private void ProcessResponseCharacters(CharactersResponse obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            if (obj.code != (int)HttpStatusCode.OK) return;
            var responseCharacters = obj.data.characters;
            if (responseCharacters.Length == 0) return;
            OnInventoryFilled(responseCharacters);
        }

        private void OnInventoryFilled(Objects.Character[] characters)
        {
            var nftCharacters = characters.Select(CreateNftCharacter).ToList();
            _heroInventory.OwnedHeroes.Clear();
            _heroInventory.OwnedHeroes = nftCharacters;
        }

        private HeroSpec CreateNftCharacter(Objects.Character characterResponse)
        {
            var nftCharacter = new HeroSpec();
            FillCharacterData(characterResponse, ref nftCharacter);
            return nftCharacter;
        }

        private void FillCharacterData(Objects.Character response, ref HeroSpec nftCharacter)
        {
            nftCharacter.Id = response.id;
            nftCharacter.Experience = (float)(response.exp);
            nftCharacter.Elemental = _elements.FirstOrDefault(element => element.Id == Int32.Parse(response.elementId));
            nftCharacter.Class = _classes.FirstOrDefault(@class => @class.Id == Int32.Parse(response.classId));
            FillCharacterStats(response, ref nftCharacter);
            nftCharacter.Origin = _charOrigins[_charNames.IndexOf(_charNames.FirstOrDefault(origin => origin == response.name))];
        }

        private void FillCharacterStats(Objects.Character response, ref HeroSpec nftCharacter)
        {
            var initialAttributes = new Dictionary<AttributeScriptableObject, CappedAttributeDef>();
            _fields ??= typeof(Objects.Character).GetFields();
            foreach (var fieldInfo in _fields)
            {
                if (_lookupAttribute.TryGetValue(fieldInfo.Name, out var attributeSO) == false) continue;
                var value = (float)fieldInfo.GetValue(response);
                if (initialAttributes.TryGetValue(attributeSO, out var def))
                {
                    if (fieldInfo.Name.Contains("min"))
                        def.MinValue = value;
                    else
                        def.MaxValue = value;
                    initialAttributes[attributeSO] = def;
                }
                else
                {
                    initialAttributes.Add(attributeSO, new CappedAttributeDef(attributeSO)
                    {
                        MinValue = fieldInfo.Name.Contains("min") ? value : -1,
                        MaxValue = fieldInfo.Name.Contains("max") ? value : -1
                    });
                }
            }

            var stats = new StatsDef
            {
                MaxLevel = response.maxLv,
                Attributes = initialAttributes.Values.ToArray()
            };

            nftCharacter.Stats = stats;
        }

        private void OnError(Exception error)
        {
            Debug.LogError("FetchProfileCharacters::OnError " + error);
        }
    }
}