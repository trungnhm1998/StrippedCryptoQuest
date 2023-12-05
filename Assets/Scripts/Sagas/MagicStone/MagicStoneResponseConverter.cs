using System.Linq;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Item.MagicStone;

namespace CryptoQuest.Sagas.MagicStone
{
    public class MagicStoneResponseConverter
    {
        private IMagicStoneDefDatabase _database;
        private PassiveAbilityDatabase _passiveAbilityDatabase;

        public MagicStoneResponseConverter(IMagicStoneDefDatabase database,
            PassiveAbilityDatabase passiveAbilityDatabase)
        {
            _passiveAbilityDatabase = passiveAbilityDatabase;
            _database = database;
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