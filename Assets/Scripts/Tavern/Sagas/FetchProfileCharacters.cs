using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Actions;
using CryptoQuest.Character;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Networking;
using CryptoQuest.API;
using CryptoQuest.Events.UI.Menu;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using TinyMessenger;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using AttributeScriptableObject =
    IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;

namespace CryptoQuest.Tavern.Sagas
{
    public class FetchProfileCharacters : MonoBehaviour
    {
        [SerializeField] private AssetReferenceT<HeroInventorySO> _heroInventoryAsset;
        private HeroInventorySO _heroInventory;
        [SerializeField] private List<Elemental> _elements = new();
        [SerializeField] private List<CharacterClass> _classes = new();

        [Tooltip("The order of character's name must match with the order of character's origin")]
        [SerializeField] private List<String> _charNames = new();

        [Tooltip("The order of character's name must match with the order of character's origin")]
        [SerializeField] private List<Origin> _charOrigins = new();

        [SerializeField] private List<ResponseAttributeMap> _attributeMap = new();
        [SerializeField] private HeroInventoryFilledEvent _inventoryFilled;

        private Dictionary<string, AttributeScriptableObject> _lookupAttribute = new();
        private FieldInfo[] _fields;

        private TinyMessageSubscriptionToken _heroInventoryUpdateEvent;
        private TinyMessageSubscriptionToken _fetchEvent;
        private TinyMessageSubscriptionToken _transferSuccessEvent;

        private void Awake()
        {
            _lookupAttribute = _attributeMap.ToDictionary(map => map.Name, map => map.Attribute);
        }

        private void OnEnable()
        {
            _fetchEvent = ActionDispatcher.Bind<FetchProfileCharactersAction>(HandleAction);
            _heroInventoryUpdateEvent = ActionDispatcher.Bind<GetGameNftCharactersSucceed>(RefreshHeroInventory);
            _transferSuccessEvent = ActionDispatcher.Bind<TransferSucceed>(FilterAndRefreshInventory);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_fetchEvent);
            ActionDispatcher.Unbind(_heroInventoryUpdateEvent);
            ActionDispatcher.Unbind(_transferSuccessEvent);
        }

        private void FilterAndRefreshInventory(TransferSucceed ctx)
        {
            var ingameCharacters =
                ctx.ResponseCharacters.Where(character => character.inGameStatus == (int)ECharacterStatus.InGame);
            OnInventoryFilled(ingameCharacters.ToArray());
        }

        private void RefreshHeroInventory(GetGameNftCharactersSucceed ctx)
        {
            OnInventoryFilled(ctx.InGameCharacters.ToArray());
        }

        private void HandleAction(FetchProfileCharactersAction _)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithParams(new Dictionary<string, string>()
                    { { "source", $"{((int)ECharacterStatus.InGame).ToString()}" } })
                .Get<CharactersResponse>(CharacterAPI.GET_CHARACTERS)
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

        private void OnInventoryFilled(CryptoQuest.Sagas.Objects.Character[] characters)
            => StartCoroutine(CoLoadAndUpdateInventory(characters));

        private IEnumerator CoLoadAndUpdateInventory(CryptoQuest.Sagas.Objects.Character[] characters)
        {
            if (_heroInventory == null)
            {
                var handle = _heroInventoryAsset.LoadAssetAsync();
                yield return handle;
                _heroInventory = handle.Result;
            }

            var nftCharacters = characters.Select(CreateNftCharacter).ToList();
            _heroInventory.OwnedHeroes.Clear();
            _heroInventory.OwnedHeroes = nftCharacters;

            _inventoryFilled.RaiseEvent(_heroInventory.OwnedHeroes);
        }

        private HeroSpec CreateNftCharacter(CryptoQuest.Sagas.Objects.Character characterResponse)
        {
            var nftCharacter = new HeroSpec();
            FillCharacterData(characterResponse, ref nftCharacter);
            return nftCharacter;
        }

        private void FillCharacterData(CryptoQuest.Sagas.Objects.Character response, ref HeroSpec nftCharacter)
        {
            var heroName = !string.IsNullOrEmpty(response.name) ? response.name : _charNames[0];
            nftCharacter.Id = response.id;
            nftCharacter.Experience = (float)(response.exp);
            nftCharacter.Elemental = _elements.FirstOrDefault(element => element.Id == Int32.Parse(response.elementId));
            nftCharacter.Class = _classes.FirstOrDefault(@class => @class.Id == Int32.Parse(response.classId));
            FillCharacterStats(response, ref nftCharacter);
            nftCharacter.Origin =
                _charOrigins[_charNames.IndexOf(_charNames.FirstOrDefault(origin => origin == heroName))];
        }

        private void FillCharacterStats(CryptoQuest.Sagas.Objects.Character response, ref HeroSpec nftCharacter)
        {
            var initialAttributes = new Dictionary<AttributeScriptableObject, CappedAttributeDef>();
            _fields ??= typeof(CryptoQuest.Sagas.Objects.Character).GetFields();
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