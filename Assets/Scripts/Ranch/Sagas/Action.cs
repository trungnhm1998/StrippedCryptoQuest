using CryptoQuest.Beast;
using IndiGames.Core.Events;

namespace CryptoQuest.Ranch.Sagas
{
    public class RequestEvolveBeast : ActionBase
    {
        public IBeast Base;
        public IBeast Material;
    }
    public class EvolveResponsed : ActionBase
    {
        public EvolveResponse Response { get; private set; }
        public RequestEvolveBeast RequestContext { get; private set; }

        public EvolveResponsed(EvolveResponse response, RequestEvolveBeast requestCtx)
        {
            Response = response;
            RequestContext = requestCtx;
        }
    }
    public class EvolveRequestFailed : ActionBase { }
    public class EvolveSucceed : ActionBase{}
    public class EvolveFailed : ActionBase{}
}