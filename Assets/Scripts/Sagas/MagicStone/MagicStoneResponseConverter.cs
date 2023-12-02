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

        public Item.MagicStone.MagicStone Convert(Objects.MagicStone responseObject)
        {
            var magicStone = new Item.MagicStone.MagicStone()
            {
                StoneData = new MagicStoneData()
                {
                    ID = responseObject.id,
                    Level = responseObject.stoneLv,
                    StoneDef = _database[responseObject.elementId]
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