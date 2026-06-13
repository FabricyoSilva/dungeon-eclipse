using UnityEngine;
using DungeonEclipse.Combat;
using DungeonEclipse.Core;

namespace DungeonEclipse.Effects
{
    /// <summary>
    /// Barra de vida em world-space (dois sprites: fundo + preenchimento),
    /// desenhada acima de um alvo que ela segue. Reutilizável para qualquer Health.
    /// </summary>
    public class WorldHealthBar : MonoBehaviour
    {
        [SerializeField] private float yOffset = 0.7f;
        [SerializeField] private float width = 0.9f;
        [SerializeField] private float height = 0.12f;
        [SerializeField] private Color bgColor = new Color(0.1f, 0.1f, 0.1f, 0.85f);
        [SerializeField] private Color fillColor = new Color(0.85f, 0.2f, 0.25f);

        private Transform _target;
        private Transform _fill;
        private Health _health;

        public void Bind(Health health, Transform target)
        {
            _target = target;
            _health = health;
            Build();
            _health.OnChanged += OnChanged;
            OnChanged(health.Current, health.Max);
        }

        private void OnDestroy()
        {
            if (_health != null) _health.OnChanged -= OnChanged;
        }

        private void Build()
        {
            var bg = MakeSprite("Bg", bgColor, 11);
            bg.localScale = new Vector3(width, height, 1f);
            bg.localPosition = Vector3.zero;

            _fill = MakeSprite("Fill", fillColor, 12);
            _fill.localScale = new Vector3(width, height * 0.75f, 1f);
            _fill.localPosition = Vector3.zero;
        }

        private Transform MakeSprite(string spriteName, Color color, int order)
        {
            var go = new GameObject(spriteName);
            go.transform.SetParent(transform, false);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = PlaceholderSprite.Square;
            sr.color = color;
            sr.sortingOrder = order;
            return go.transform;
        }

        private void OnChanged(int current, int max)
        {
            float pct = max > 0 ? Mathf.Clamp01((float)current / max) : 0f;
            float w = width * pct;
            // pivot central: deslocar para que a borda esquerda fique fixa
            _fill.localScale = new Vector3(w, height * 0.75f, 1f);
            _fill.localPosition = new Vector3(-width / 2f + w / 2f, 0f, 0f);
        }

        private void LateUpdate()
        {
            if (_target == null) { Destroy(gameObject); return; }
            transform.position = _target.position + new Vector3(0f, yOffset, 0f);
        }
    }
}
