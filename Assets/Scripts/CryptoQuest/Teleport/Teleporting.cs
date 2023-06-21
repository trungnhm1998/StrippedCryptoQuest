using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CryptoQuest
{
    public class Teleporting : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Teleport(_sceneName);
            }
        }

        private void Teleport(string scene)
        {
            SceneManager.LoadScene(scene);
            Debug.Log(scene);
        }
    }
}