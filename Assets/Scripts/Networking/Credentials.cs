using UnityEngine;

namespace CryptoQuest.Networking
{
    /// <summary>
    /// This act as in memory data only "reducer" for authentication
    /// </summary>
    [CreateAssetMenu(menuName = "Crypto Quest/Networking/Credentials", fileName = "Credentials", order = 0)]
    public class Credentials : ScriptableObject
    {
        [field: SerializeField] public string UUID { get; set; }
        [field: SerializeField] public string Wallet { get; set; }
        [field: SerializeField] public string Token { get; set; }
        [field: SerializeField] public string RefreshToken { get; set; }
        
        [SerializeField, Header("UserPrefs")] private string _tokenKey = "token";
        [SerializeField] private string _refreshTokenKey = "refresh_token";

        /// <summary>
        /// Check access token to validate if user has logged in
        /// </summary>
        public bool IsLoggedIn() => string.IsNullOrEmpty(Token) == false && string.IsNullOrEmpty(RefreshToken) == false;
        
        public void Load()
        {
            Token = PlayerPrefs.GetString(_tokenKey,
#if UNITY_EDITOR
                Token
#else
                string.Empty
#endif
                );
            RefreshToken = PlayerPrefs.GetString(_refreshTokenKey,
#if UNITY_EDITOR
                RefreshToken
#else
                string.Empty
#endif 
                );
        }

        public void Save()
        {
            PlayerPrefs.SetString(_tokenKey, Token);
            PlayerPrefs.SetString(_refreshTokenKey, RefreshToken);
        }

        public void Reset()
        {
            Wallet = string.Empty;
            Token = string.Empty;
            RefreshToken = string.Empty;
            Save();
        }
    }
}