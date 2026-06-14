using System.Collections;
using UnityEngine;
using DungeonEclipse.Combat;

namespace DungeonEclipse.Effects
{
    /// <summary>
    /// Pisca o SpriteRenderer em vermelho ao detectar queda de vida.
    /// Assina Health.OnChanged. Anexar à GameObject que tem o SpriteRenderer.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class DamageFlash : MonoBehaviour
    {
        [SerializeField] private Color flashColor = Color.red;
        [SerializeField] private float duration = 0.12f;

        private SpriteRenderer _sr;
        private Color _base;
        private Health _health;
        private int _last;
        private Coroutine _running;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            _base = _sr.color;
        }

        public void Bind(Health health)
        {
            _health = health;
            _last = health.Current;
            _health.OnChanged += OnChanged;
        }

        private void OnDestroy()
        {
            if (_health != null) _health.OnChanged -= OnChanged;
        }

        private void OnChanged(int current, int max)
        {
            if (current < _last)
            {
                if (_running != null) StopCoroutine(_running);
                _running = StartCoroutine(Flash());
            }
            _last = current;
        }

        private IEnumerator Flash()
        {
            _sr.color = flashColor;
            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                _sr.color = Color.Lerp(flashColor, _base, t / duration);
                yield return null;
            }
            _sr.color = _base;
            _running = null;
        }
    }
}
