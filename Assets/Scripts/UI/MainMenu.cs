using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DungeonEclipse.UI
{
    /// <summary>Menu inicial construído por código: título + Começar/Sair.</summary>
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private string firstScene = "Andar1";

        private static Font UiFont =>
            Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        private void Start()
        {
            ScreenFader.Ensure().FadeIn();
            Build();
        }

        private void Build()
        {
            if (FindObjectOfType<EventSystem>() == null)
            {
                var es = new GameObject("EventSystem");
                es.AddComponent<EventSystem>();
                es.AddComponent<StandaloneInputModule>();
            }

            var canvasGo = new GameObject("MenuCanvas");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            canvasGo.AddComponent<GraphicRaycaster>();

            var bg = new GameObject("Bg").AddComponent<Image>();
            bg.transform.SetParent(canvasGo.transform, false);
            bg.color = new Color(0.05f, 0.04f, 0.09f);
            var brt = bg.rectTransform;
            brt.anchorMin = Vector2.zero; brt.anchorMax = Vector2.one;
            brt.offsetMin = Vector2.zero; brt.offsetMax = Vector2.zero;

            var title = MakeText(canvasGo.transform, "DUNGEON ECLIPSE", 64, new Vector2(0, 150));
            title.color = new Color(1f, 0.84f, 0.3f);

            var subtitle = MakeText(canvasGo.transform, "Luz e Escuridão", 28, new Vector2(0, 90));
            subtitle.color = new Color(0.8f, 0.8f, 0.9f);

            MakeButton(canvasGo.transform, "Começar", new Vector2(0, -20),
                () => ScreenFader.Ensure().FadeToScene(firstScene));
            MakeButton(canvasGo.transform, "Sair", new Vector2(0, -110), Quit);
        }

        private void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private Text MakeText(Transform parent, string content, int size, Vector2 pos)
        {
            var go = new GameObject("Text");
            go.transform.SetParent(parent, false);
            var txt = go.AddComponent<Text>();
            txt.text = content;
            txt.font = UiFont;
            txt.fontSize = size;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = Color.white;
            txt.horizontalOverflow = HorizontalWrapMode.Overflow;
            var rt = txt.rectTransform;
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(900, size + 20);
            rt.anchoredPosition = pos;
            return txt;
        }

        private void MakeButton(Transform parent, string label, Vector2 pos, UnityEngine.Events.UnityAction onClick)
        {
            var btnGo = new GameObject(label + "Button");
            btnGo.transform.SetParent(parent, false);
            var img = btnGo.AddComponent<Image>();
            img.color = new Color(0.9f, 0.75f, 0.2f);
            var btn = btnGo.AddComponent<Button>();
            btn.targetGraphic = img;
            btn.onClick.AddListener(onClick);
            var rt = img.rectTransform;
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(260, 64);
            rt.anchoredPosition = pos;

            var txt = MakeText(btnGo.transform, label, 30, Vector2.zero);
            txt.color = Color.black;
            txt.rectTransform.sizeDelta = new Vector2(260, 64);
        }
    }
}
