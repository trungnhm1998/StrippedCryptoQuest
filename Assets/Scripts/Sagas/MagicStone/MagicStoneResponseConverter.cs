using System.Linq;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Item.MagicStone;
using IndiGames.Core.Common;
using UnityEngine;

namespace CryptoQuest.Sagas.MagicStone
{
    public interface IMagicStoneResponseConverter
    {
        IMagicStone Convert(Objects.MagicStone responseObject);
    }

    public class MagicStoneResponseConverter : MonoBehaviour, IMagicStoneResponseConverter
    {
        [SerializeField] private MagicStoneDefinitionDatabase _database;
        [SerializeField] private PassiveAbilityDatabase _passiveAbilityDatabase;

        private void Awake()
        {
            ServiceProvider.Provide<IMagicStoneResponseConverter>(this);
        }

        public IMagicStone Convert(Objects.MagicStone responseObject)
        {
            var magicStone = new Item.MagicStone.MagicStone()
            {
                Data = new MagicStoneData()
                {
                    ID = responseObject.id,
                    Level = responseObject.stoneLv,
                    Def = _database[responseObject.elementId],
                    AttachEquipmentId = responseObject.attachEquipment
                }
            };
            var passiveList = magicStone.Passives.ToList();
            passiveList.Add(_passiveAbilityDatabase.GetDataById(responseObject.passiveSkillId1));
            passiveList.Add(_passiveAbilityDatabase.GetDataById(responseObject.passiveSkillId2));
            magicStone.Passives = passiveList.ToArray();
            return magicStone;
        }
    }
}