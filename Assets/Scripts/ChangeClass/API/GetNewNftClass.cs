using CryptoQuest.ChangeClass.View;
using IndiGames.Core.Events;

namespace CryptoQuest.ChangeClass.API
{
    public class GetNewNftClass : ActionBase
    {
        public int BaseUnitId1;
        public int BaseUnitId2;
        public UIOccupation Occupation;
        public GetNewNftClass(int baseUnitId1, int baseUnitId2, UIOccupation occupation)
        {
            BaseUnitId1 = baseUnitId1;
            BaseUnitId2 = baseUnitId2;
            Occupation = occupation;
        }
    }
    public class GetNewNftClassBerserker : ActionBase
    {
        public int BaseUnitId;

        public GetNewNftClassBerserker(int baseId)
        {
            BaseUnitId = baseId;
        }
    }

    public class ChangeNewClassDataResponse : ActionBase
    {
        public NewCharacter ResponseData;

        public ChangeNewClassDataResponse(NewCharacter response)
        {
            ResponseData = response;
        }
    }

}
