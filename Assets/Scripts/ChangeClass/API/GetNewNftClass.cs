using CryptoQuest.Core;

namespace CryptoQuest.ChangeClass.API
{
    public class GetNewNftClass : ActionBase
    {
        public bool ForceRefresh { get; set; } = false;
    }

    public class GetMockNftClassData : ActionBase
    {
        public bool ForceRefresh { get; set; } = false;
    }

    public class GetNftClassesSucceed : ActionBase { }

    public class GetNftClassesFailed : ActionBase { }

}
