using UnityEngine;

namespace CryptoQuest.Battle.VFX
{
    public class MoveTowardDirection : MonoBehaviour
    {
        [SerializeField] private Vector3 _direction = new Vector3(0.8f, 0, 0);
        
        private void Update()
        {
            transform.position += _direction * Time.deltaTime;
        }
    }
}