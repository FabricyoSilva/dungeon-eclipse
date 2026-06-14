using System.Collections;
using UnityEngine;
using DungeonEclipse.Core;

namespace DungeonEclipse.Effects
{
    /// <summary>
    /// Estouro de partículas placeholder (quadradinhos que se afastam e somem),
    /// gerado por código. Use HitEffect.Burst(...) ao destruir/golpear.
    /// </summary>
    public class HitEffect : MonoBehaviour
    {
        public static void Burst(Vector3 position, Color color, int count = 8)
        {
            var host = new GameObject("HitEffect");
            host.transform.position = position;
            host.AddComponent<HitEffect>().StartCoroutine_(color, count);
        }

        private void StartCoroutine_(Color color, int count)
            => StartCoroutine(Run(color, count));

        private IEnumerator Run(Color color, int count)
        {
            var parts = new SpriteRenderer[count];
            var dirs = new Vector2[count];
            for (int i = 0; i < count; i++)
            {
                var go = new GameObject("p");
                go.transform.SetParent(transform);
                go.transform.position = transform.position;
                go.transform.localScale = Vector3.one * 0.18f;
                var sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = PlaceholderSprite.Square;
                sr.color = color;
                sr.sortingOrder = 10;
                parts[i] = sr;
                float ang = (360f / count) * i * Mathf.Deg2Rad;
                dirs[i] = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang));
            }

            const float dur = 0.35f;
            float t = 0f;
            while (t < dur)
            {
                t += Time.deltaTime;
                float k = t / dur;
                for (int i = 0; i < count; i++)
                {
                    if (parts[i] == null) continue;
                    parts[i].transform.position += (Vector3)(dirs[i] * (2.5f * Time.deltaTime));
                    var c = parts[i].color;
                    c.a = 1f - k;
                    parts[i].color = c;
                }
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
