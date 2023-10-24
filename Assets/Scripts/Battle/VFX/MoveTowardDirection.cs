using UnityEngine;

namespace CryptoQuest.Battle.VFX
{
    public class MoveTowardDirection : MonoBehaviour
    {
        [SerializeField] private Vector3 _direction;
        
        private void Update()
        {
            transform.position += _direction * Time.deltaTime;
        }
    }
}