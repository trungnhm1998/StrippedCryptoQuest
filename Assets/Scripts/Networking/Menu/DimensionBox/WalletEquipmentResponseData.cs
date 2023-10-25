using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Networking.Menu.DimensionBox
{
    public class WalletEquipmentResponseData
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("gold")]
        public int Gold { get; set; }

        [JsonProperty("diamond")]
        public double Diamond { get; set; }

        [JsonProperty("soul")]
        public int Soul { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("data")]
        public EquipmentData Data { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("total_page")]
        public int TotalPage { get; set; }
    }

    public class EquipmentData
    {
        [JsonProperty("equipments")]
        public List<Equipment> Equipments { get; set; }
    }

    public class Equipment
    {
        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("equipmentTokenId")]
        public string EquipmentTokenId { get; set; }

        [JsonProperty("inGameStatus")]
        public int InGameStatus { get; set; }

        [JsonProperty("lv")]
        public int Lv { get; set; }

        [JsonProperty("equipmentId")]
        public string EquipmentId { get; set; }

        [JsonProperty("HP")]
        public int HP { get; set; }

        [JsonProperty("addHp")]
        public int AddHp { get; set; }

        [JsonProperty("maxHp")]
        public int MaxHp { get; set; }

        [JsonProperty("MP")]
        public int MP { get; set; }

        [JsonProperty("addMp")]
        public int AddMp { get; set; }

        [JsonProperty("maxMp")]
        public int MaxMp { get; set; }

        [JsonProperty("strength")]
        public int Strength { get; set; }

        [JsonProperty("addStrength")]
        public int AddStrength { get; set; }

        [JsonProperty("vitality")]
        public int Vitality { get; set; }

        [JsonProperty("addVitality")]
        public int AddVitality { get; set; }

        [JsonProperty("agility")]
        public int Agility { get; set; }

        [JsonProperty("addAgility")]
        public int AddAgility { get; set; }

        [JsonProperty("intelligence")]
        public int Intelligence { get; set; }

        [JsonProperty("addIntelligence")]
        public int AddIntelligence { get; set; }

        [JsonProperty("luck")]
        public int Luck { get; set; }

        [JsonProperty("addLuck")]
        public int AddLuck { get; set; }

        [JsonProperty("MATK")]
        public int MATK { get; set; }

        [JsonProperty("addMATK")]
        public int AddMATK { get; set; }

        [JsonProperty("deffence")]
        public int Deffence { get; set; }

        [JsonProperty("addDeffence")]
        public int AddDeffence { get; set; }

        [JsonProperty("attack")]
        public int Attack { get; set; }

        [JsonProperty("addAttack")]
        public int AddAttack { get; set; }

        [JsonProperty("attachUnitTokenId")]
        public string AttachUnitTokenId { get; set; }

        [JsonProperty("equipNameJp")]
        public string EquipNameJp { get; set; }

        [JsonProperty("descriptionJp")]
        public string DescriptionJp { get; set; }

        [JsonProperty("equipTypeNameJp")]
        public string EquipTypeNameJp { get; set; }

        [JsonProperty("seriesNameJp")]
        public string SeriesNameJp { get; set; }

        [JsonProperty("equipName")]
        public string EquipName { get; set; }

        [JsonProperty("descriptionEn")]
        public string DescriptionEn { get; set; }

        [JsonProperty("equipTypeName")]
        public string EquipTypeName { get; set; }

        [JsonProperty("rarityName")]
        public string RarityName { get; set; }

        [JsonProperty("categoryId")]
        public string CategoryId { get; set; }

        [JsonProperty("seriesName")]
        public string SeriesName { get; set; }

        [JsonProperty("equipPartId")]
        public string EquipPartId { get; set; }

        [JsonProperty("equipTypeId")]
        public int EquipTypeId { get; set; }

        [JsonProperty("rarityId")]
        public int RarityId { get; set; }

        [JsonProperty("seriesId")]
        public string SeriesId { get; set; }

        [JsonProperty("groupId")]
        public string GroupId { get; set; }

        [JsonProperty("evoLv")]
        public int EvoLv { get; set; }

        [JsonProperty("restrictedLv")]
        public int RestrictedLv { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("sellingPrice")]
        public int SellingPrice { get; set; }

        [JsonProperty("evasionRate")]
        public int EvasionRate { get; set; }

        [JsonProperty("criticalRate")]
        public int CriticalRate { get; set; }

        [JsonProperty("maxLv")]
        public int MaxLv { get; set; }

        [JsonProperty("maxHP")]
        public int MaxHP { get; set; }

        [JsonProperty("maxMP")]
        public int MaxMP { get; set; }

        [JsonProperty("maxStrength")]
        public int MaxStrength { get; set; }

        [JsonProperty("maxVitality")]
        public int MaxVitality { get; set; }

        [JsonProperty("maxAgility")]
        public int MaxAgility { get; set; }

        [JsonProperty("maxIntelligence")]
        public int MaxIntelligence { get; set; }

        [JsonProperty("maxLuck")]
        public int MaxLuck { get; set; }

        [JsonProperty("maxAttack")]
        public int MaxAttack { get; set; }

        [JsonProperty("maxMATK")]
        public int MaxMATK { get; set; }

        [JsonProperty("maxDeffence")]
        public int MaxDeffence { get; set; }

        [JsonProperty("maxEvasionRate")]
        public int MaxEvasionRate { get; set; }

        [JsonProperty("maxCriticalRate")]
        public int MaxCriticalRate { get; set; }

        [JsonProperty("miningPower")]
        public int MiningPower { get; set; }

        [JsonProperty("consumeFuel")]
        public int ConsumeFuel { get; set; }

        [JsonProperty("imageFileName")]
        public string ImageFileName { get; set; }

        [JsonProperty("imageURL")]
        public string ImageURL { get; set; }

        [JsonProperty("nft")]
        public int Nft { get; set; }

        [JsonProperty("star")]
        public int Star { get; set; }

    }
}
