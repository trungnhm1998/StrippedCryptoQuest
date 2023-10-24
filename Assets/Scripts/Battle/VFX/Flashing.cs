using UnityEngine;

namespace CryptoQuest.Battle.VFX
{
    public class Flashing : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private void OnValidate() => _spriteRenderer = GetComponent<SpriteRenderer>();
        private void Update() => _spriteRenderer.color = Color.Lerp(_spriteRenderer.color, Color.clear, Time.deltaTime);
    }
}