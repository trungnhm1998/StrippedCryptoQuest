using System.Collections;
using System.Linq;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Item.MagicStone;
using IndiGames.Core.Common;
using UnityEngine;
using MagicStoneItem = CryptoQuest.Item.MagicStone.MagicStone;

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
            var magicStone = new MagicStoneItem()
            {
                Data = new MagicStoneData()
                {
                    ID = responseObject.id,
                    Level = responseObject.stoneLv,
                    Def = _database[responseObject.elementId],
                    AttachEquipmentId = responseObject.attachEquipment
                }
            };

            StartCoroutine(CoLoadPassiveAsync(magicStone, responseObject));
            return magicStone;
        }

        private IEnumerator CoLoadPassiveAsync(MagicStoneItem magicStone, Objects.MagicStone response)
        {
            yield return _passiveAbilityDatabase.LoadDataById(response.passiveSkillId1);
            yield return _passiveAbilityDatabase.LoadDataById(response.passiveSkillId2);

            var passiveList = magicStone.Passives.ToList();
            passiveList.Add(_passiveAbilityDatabase.GetDataById(response.passiveSkillId1));
            passiveList.Add(_passiveAbilityDatabase.GetDataById(response.passiveSkillId2));
            magicStone.Passives = passiveList.ToArray();
        }
    }
}