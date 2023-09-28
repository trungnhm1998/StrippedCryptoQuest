using CryptoQuest.SocialLogin;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Events
{
    [CreateAssetMenu(menuName = "Core/Events/Login Info Event Channel")]
    public class AuthenticateFormInfoEventChannelSO : GenericEventChannelSO<AuthenticateFormInfo> { }
}