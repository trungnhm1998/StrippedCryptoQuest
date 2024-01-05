using System.Net;
using CryptoQuest.Networking;
using IndiGames.Core.Events;

namespace CryptoQuest.UI.Popups.Sagas
{
    public class ResponseWithErrorSaga : SagaBase<ResponseWithError>
    {
        protected override void HandleAction(ResponseWithError ctx)
        {
            if (ctx.RequestException.StatusCode == (long)HttpStatusCode.ServiceUnavailable)
            {
                ActionDispatcher.Dispatch(new ServerMaintenancePopup());
            }
            else
            {
                ActionDispatcher.Dispatch(new ClientErrorPopup());
            }
        }
    }
}