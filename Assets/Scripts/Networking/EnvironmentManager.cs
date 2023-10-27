using CryptoQuest.Environment;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Networking
{
    public class EnvironmentManager : MonoBehaviour
    {
        [SerializeField] private EnvironmentSO _environment;
        private void Awake() => ServiceProvider.Provide(_environment);
    }
}