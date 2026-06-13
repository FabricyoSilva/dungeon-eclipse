using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DungeonEclipse.UI
{
    /// <summary>
    /// Overlay preto persistente (singleton) para fade-in ao abrir a cena e
    /// fade-out antes de trocar de cena. Usa tempo não-escalado (funciona com
    /// Time.timeScale = 0).
    /// </summary>
    public class ScreenFader : MonoBehaviour
    {
        public static ScreenFader Instance { get; private set; }
        private Image _img;

        public static ScreenFader Ensure()
        {
            if (Instance != null) return Instance;
            var go = new GameObject("ScreenFader");
            DontDestroyOnLoad(go);
            Instance = go.AddComponent<ScreenFader>();
            Instance.Build();
            return Instance;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Build()
        {
            var canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999;
            var imgGo = new GameObject("Fade");
            imgGo.transform.SetParent(transform, false);
            _img = imgGo.AddComponent<Image>();
            _img.color = Color.black;
            _img.raycastTarget = false;
            var rt = _img.rectTransform;
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
        }

        public void FadeIn(float dur = 0.4f) => StartCoroutine(Fade(1f, 0f, dur));

        public void FadeToScene(string scene, float dur = 0.4f)
            => StartCoroutine(FadeOutLoad(scene, dur));

        private IEnumerator FadeOutLoad(string scene, float dur)
        {
            yield return Fade(0f, 1f, dur);
            Time.timeScale = 1f;
            SceneManager.LoadScene(scene);
        }

        private IEnumerator Fade(float from, float to, float dur)
        {
            var c = _img.color;
            float t = 0f;
            while (t < dur)
            {
                t += Time.unscaledDeltaTime;
                c.a = Mathf.Lerp(from, to, t / dur);
                _img.color = c;
                yield return null;
            }
            c.a = to;
            _img.color = c;
        }
    }
}
