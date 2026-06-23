using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DungeonEclipse.Combat;

namespace DungeonEclipse.Core
{
    /// <summary>
    /// HUD construído por código: barra de vida, mensagens curtas e painel de
    /// vitória com botão Reiniciar.
    /// </summary>
    public class HudController : MonoBehaviour
    {
        private Image _healthFill;
        private Text _message;
        private GameObject _victoryPanel;
        private GameObject _defeatPanel;
        private GameObject _introPanel;
        private Text _introText;
        private bool _introOpen;
        private Health _health;
        private float _messageTimer;

        private static Font UiFont =>
            Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        private void Awake()
        {
            BuildUi();
            Messages.OnMessage += ShowMessage;
        }

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnVictory += ShowVictory;
                GameManager.Instance.OnDefeat += ShowDefeat;
            }
        }

        private void OnDestroy()
        {
            Messages.OnMessage -= ShowMessage;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnVictory -= ShowVictory;
                GameManager.Instance.OnDefeat -= ShowDefeat;
            }
        }

        public void Bind(Health health)
        {
            _health = health;
            _health.OnChanged += UpdateBar;
            UpdateBar(_health.Current, _health.Max);
        }

        /// <summary>Mostra o painel de introdução e pausa o jogo até ser dispensado.</summary>
        public void ShowIntro(string text)
        {
            _introText.text = text;
            _introPanel.SetActive(true);
            _introOpen = true;
            Time.timeScale = 0f;
        }

        private void HideIntro()
        {
            _introOpen = false;
            _introPanel.SetActive(false);
            Time.timeScale = 1f;
        }

        private void Update()
        {
            if (_introOpen)
            {
                // Dispensa com Enter ou clique (evita colidir com Espaço/ação de jogo)
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)
                    || Input.GetMouseButtonDown(0))
                    HideIntro();
                return;
            }

            if (_messageTimer > 0f)
            {
                _messageTimer -= Time.deltaTime;
                if (_messageTimer <= 0f) _message.text = "";
            }
        }

        private void ShowMessage(string text)
        {
            _message.text = text;
            _messageTimer = 3f;
        }

        private void ShowVictory() => _victoryPanel.SetActive(true);

        private void ShowDefeat() => _defeatPanel.SetActive(true);

        private void UpdateBar(int current, int max)
        {
            float pct = max > 0 ? (float)current / max : 0f;
            _healthFill.rectTransform.anchorMax = new Vector2(pct, 1f);
        }

        private void BuildUi()
        {
            EnsureEventSystem();

            var canvasGo = new GameObject("HUDCanvas");
            canvasGo.transform.SetParent(transform);
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            canvasGo.AddComponent<GraphicRaycaster>();

            // Barra de vida (fundo)
            var barBg = CreateImage("HealthBg", canvasGo.transform,
                new Color(0.1f, 0.1f, 0.1f, 0.8f));
            Anchor(barBg.rectTransform, new Vector2(0, 1), new Vector2(0, 1),
                new Vector2(0, 1), new Vector2(120, -34), new Vector2(220, 28));

            // Barra de vida (preenchimento, ancorado para encolher pela direita)
            _healthFill = CreateImage("HealthFill", barBg.transform,
                new Color(0.85f, 0.2f, 0.25f));
            var ft = _healthFill.rectTransform;
            ft.anchorMin = Vector2.zero; ft.anchorMax = Vector2.one;
            ft.offsetMin = Vector2.zero; ft.offsetMax = Vector2.zero;

            // Texto de mensagem
            _message = CreateText("Message", canvasGo.transform, "", 28, TextAnchor.UpperCenter);
            Anchor(_message.rectTransform, new Vector2(0.5f, 1), new Vector2(0.5f, 1),
                new Vector2(0.5f, 1), new Vector2(0, -70), new Vector2(1000, 50));

            // Painel de vitória
            _victoryPanel = new GameObject("VictoryPanel");
            _victoryPanel.transform.SetParent(canvasGo.transform, false);
            var vpImg = _victoryPanel.AddComponent<Image>();
            vpImg.color = new Color(0f, 0f, 0f, 0.75f);
            var vt = vpImg.rectTransform;
            vt.anchorMin = Vector2.zero; vt.anchorMax = Vector2.one;
            vt.offsetMin = Vector2.zero; vt.offsetMax = Vector2.zero;

            var winText = CreateText("WinText", _victoryPanel.transform,
                "O Eclipse se Desfez", 52, TextAnchor.MiddleCenter);
            Anchor(winText.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0, 170), new Vector2(900, 90));
            winText.color = new Color(1f, 0.85f, 0.3f);

            var endingText = CreateText("EndingText", _victoryPanel.transform,
                "Kael ergueu o último fragmento e o Coração Eclipse voltou a pulsar. " +
                "A luz percorreu os corredores e dissolveu a corrupção que aprisionava a dungeon. " +
                "O equilíbrio entre luz e escuridão foi restaurado.\n\n" +
                "Obrigado por jogar Dungeon Eclipse.",
                26, TextAnchor.UpperCenter);
            endingText.horizontalOverflow = HorizontalWrapMode.Wrap;
            endingText.color = new Color(0.9f, 0.9f, 0.95f);
            Anchor(endingText.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0, 20), new Vector2(820, 260));

            var btnGo = new GameObject("MenuButton");
            btnGo.transform.SetParent(_victoryPanel.transform, false);
            var btnImg = btnGo.AddComponent<Image>();
            btnImg.color = new Color(0.9f, 0.75f, 0.2f);
            var btn = btnGo.AddComponent<Button>();
            btn.targetGraphic = btnImg;
            btn.onClick.AddListener(() =>
            {
                if (GameManager.Instance != null) GameManager.Instance.VoltarAoMenu();
            });
            Anchor(btnImg.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0, -170), new Vector2(260, 64));
            var btnText = CreateText("Text", btnGo.transform, "Voltar ao Menu", 28, TextAnchor.MiddleCenter);
            btnText.color = Color.black;
            var bt = btnText.rectTransform;
            bt.anchorMin = Vector2.zero; bt.anchorMax = Vector2.one;
            bt.offsetMin = Vector2.zero; bt.offsetMax = Vector2.zero;

            _victoryPanel.SetActive(false);

            // Painel de derrota (gêmeo do de vitória)
            _defeatPanel = new GameObject("DefeatPanel");
            _defeatPanel.transform.SetParent(canvasGo.transform, false);
            var dpImg = _defeatPanel.AddComponent<Image>();
            dpImg.color = new Color(0.15f, 0f, 0f, 0.8f);
            var dt = dpImg.rectTransform;
            dt.anchorMin = Vector2.zero; dt.anchorMax = Vector2.one;
            dt.offsetMin = Vector2.zero; dt.offsetMax = Vector2.zero;

            var loseText = CreateText("LoseText", _defeatPanel.transform,
                "Tente Novamente", 56, TextAnchor.MiddleCenter);
            Anchor(loseText.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0, 60), new Vector2(800, 100));
            loseText.color = new Color(1f, 0.5f, 0.4f);

            var dBtnGo = new GameObject("RestartButton");
            dBtnGo.transform.SetParent(_defeatPanel.transform, false);
            var dBtnImg = dBtnGo.AddComponent<Image>();
            dBtnImg.color = new Color(0.9f, 0.4f, 0.3f);
            var dBtn = dBtnGo.AddComponent<Button>();
            dBtn.targetGraphic = dBtnImg;
            dBtn.onClick.AddListener(() =>
            {
                if (GameManager.Instance != null) GameManager.Instance.Reiniciar();
            });
            Anchor(dBtnImg.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0, -50), new Vector2(220, 60));
            var dBtnText = CreateText("Text", dBtnGo.transform, "Reiniciar", 28, TextAnchor.MiddleCenter);
            dBtnText.color = Color.black;
            var dbt = dBtnText.rectTransform;
            dbt.anchorMin = Vector2.zero; dbt.anchorMax = Vector2.one;
            dbt.offsetMin = Vector2.zero; dbt.offsetMax = Vector2.zero;

            _defeatPanel.SetActive(false);

            // Painel de introdução
            _introPanel = new GameObject("IntroPanel");
            _introPanel.transform.SetParent(canvasGo.transform, false);
            var ipImg = _introPanel.AddComponent<Image>();
            ipImg.color = new Color(0f, 0f, 0.05f, 0.85f);
            var it = ipImg.rectTransform;
            it.anchorMin = Vector2.zero; it.anchorMax = Vector2.one;
            it.offsetMin = Vector2.zero; it.offsetMax = Vector2.zero;

            _introText = CreateText("IntroText", _introPanel.transform, "", 30, TextAnchor.MiddleCenter);
            _introText.horizontalOverflow = HorizontalWrapMode.Wrap;
            var itt = _introText.rectTransform;
            itt.anchorMin = new Vector2(0.5f, 0.5f);
            itt.anchorMax = new Vector2(0.5f, 0.5f);
            itt.pivot = new Vector2(0.5f, 0.5f);
            itt.sizeDelta = new Vector2(900, 400);
            itt.anchoredPosition = Vector2.zero;

            _introPanel.SetActive(false);
        }

        private static void EnsureEventSystem()
        {
            if (FindObjectOfType<EventSystem>() != null) return;
            var es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }

        private static Image CreateImage(string name, Transform parent, Color color)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var img = go.AddComponent<Image>();
            img.color = color;
            return img;
        }

        private static Text CreateText(string name, Transform parent, string content,
            int size, TextAnchor anchor)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var txt = go.AddComponent<Text>();
            txt.text = content;
            txt.font = UiFont;
            txt.fontSize = size;
            txt.alignment = anchor;
            txt.color = Color.white;
            txt.horizontalOverflow = HorizontalWrapMode.Overflow;
            return txt;
        }

        // anchoredPos é interpretado relativo ao pivot; aqui pivot = (anchor) p/ posicionar por canto.
        private static void Anchor(RectTransform rt, Vector2 min, Vector2 max,
            Vector2 pivot, Vector2 anchoredPos, Vector2 size)
        {
            rt.anchorMin = min;
            rt.anchorMax = max;
            rt.pivot = pivot;
            rt.sizeDelta = size;
            rt.anchoredPosition = anchoredPos;
        }
    }
}
