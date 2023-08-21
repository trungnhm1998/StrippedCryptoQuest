using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine;

namespace CryptoQuest.Data
{
    public static class DataValidator
    {
        public static bool MonsterDataValidator(MonsterUnitDataModel data)
        {
            var properties = data.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(data);
                if (value == null || !IsMatchCustomBusinessRule(value))
                    return false;
            }

            return true;
        }

        public static bool IsStringsNotNull(string[] datas, List<int> columnsException = null)
        {
            for (int i = 0; i < datas.Length; i++)
            {
                if (columnsException != null && columnsException.Contains(i))
                    continue;
                if (string.IsNullOrEmpty(datas[i])) return false;
            }

            return true;
        }

        public static bool IsValidNumberOfMonsterSetup(List<string> battleIds)
        {
            int count = 0;
            foreach (var battleId in battleIds)
            {
                if (!string.IsNullOrEmpty(battleId))
                {
                    count += battleId.Split(',').Length;
                }
            }

            Debug.Log(count);

            return count <= 4;
        }

        public static bool MonsterPartyValidator(MonsterPartyDataModel data)
        {
            var properties = data.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(data);
                if (value == null || !IsMatchCustomBusinessRule(value))
                    return false;
            }

            return true;
        }

        private static bool IsMatchCustomBusinessRule(object value)
        {
            if (value is string || value is char || value is short)
                return !string.IsNullOrEmpty((string)value);

            if (value is int)
                return ((int)value) >= 0;

            if (value is double)
                return ((double)value) >= 0;

            if (value is float)
                return ((float)value) >= 0;
            return false;
        }
    }


    public class MonsterUnitDataModel
    {
        public int MonsterId { get; set; }
        public string MonsterName { get; set; }
        public int ElementId { get; set; }
        public float HP { get; set; }
        public float MP { get; set; }
        public float Strength { get; set; }
        public float Vitality { get; set; }
        public float Agility { get; set; }
        public float Intelligence { get; set; }
        public float Luck { get; set; }
        public float Attack { get; set; }
        public float SkillPower { get; set; }
        public float Defense { get; set; }
        public float EvasionRate { get; set; }
        public float CriticalRate { get; set; }
        public float Exp { get; set; }
        public float Gold { get; set; }
        public string DropItemID { get; set; }
    }

    public class MonsterPartyDataModel
    {
        public int MonserPartyId { get; set; }
        public string MonsterGroupingProperty { get; set; }
    }
}