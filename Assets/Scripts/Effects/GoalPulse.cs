using UnityEngine;

namespace DungeonEclipse.Effects
{
    /// <summary>Pulso suave de cor e escala para destacar a sala-alvo.
    /// Anexar à GameObject do Goal (que tem um SpriteRenderer).</summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class GoalPulse : MonoBehaviour
    {
        [SerializeField] private float speed = 3f;
        [SerializeField] private float scaleAmp = 0.12f;

        private SpriteRenderer _sr;
        private Color _base;
        private Vector3 _baseScale;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            _base = _sr.color;
            _baseScale = transform.localScale;
        }

        private void Update()
        {
            float k = 0.5f + 0.5f * Mathf.Sin(Time.time * speed);
            transform.localScale = _baseScale * (1f + scaleAmp * k);
            _sr.color = Color.Lerp(_base, Color.white, 0.4f * k);
        }
    }
}
