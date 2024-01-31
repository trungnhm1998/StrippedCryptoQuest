using System;
using CryptoQuest.API;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Networking;
using IndiGames.Core.Common;
using Newtonsoft.Json;
using UniRx;

namespace CryptoQuest.Battle.Components
{
    [Serializable]
    public class ModifyEquippingState
    {
        [JsonProperty("attachId")]
        public int CharacterId;

        [JsonProperty("id")]
        public int EquipmentId;
    }

    [Serializable]
    public class ModifyResponse
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public float diamond;
        public int soul;
        public long time;
    }

    public class EquipmentsNetworkController : CharacterComponentBase
    {
        private EquipmentsController _equipmentsController;
        private HeroBehaviour _hero;
        private IRestClient _restclient;

        private void Start()
        {
            _restclient = ServiceProvider.GetService<IRestClient>();
        }

        private void OnEnable()
        {
            _equipmentsController = GetComponent<EquipmentsController>();
            _hero = GetComponent<HeroBehaviour>();

            _equipmentsController.Equipped += OnEquipping;
            _equipmentsController.Removed += OnRemoving;
        }

        private void OnDisable()
        {
            _equipmentsController.Equipped -= OnEquipping;
            _equipmentsController.Removed -= OnRemoving;
        }

        private void OnEquipping(IEquipment item)
        {
            _restclient
                .WithBody(new ModifyEquippingState()
                {
                    EquipmentId = item.Id,
                    CharacterId = _hero.Spec.Id
                })
                .Put<ModifyResponse>(EquipmentAPI.EQUIPMENTS)
                .Subscribe();
        }

        private void OnRemoving(IEquipment item)
        {
            _restclient
                .WithBody(new ModifyEquippingState()
                {
                    EquipmentId = item.Id,
                    CharacterId = 0
                })
                .Put<ModifyResponse>(EquipmentAPI.EQUIPMENTS)
                .Subscribe();
        }
    }
}